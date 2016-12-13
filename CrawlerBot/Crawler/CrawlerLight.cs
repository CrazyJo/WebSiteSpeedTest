//#define DevMode
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CrawlerBot.Core;
using CrawlerBot.Core.Parsers;
using CrawlerBot.Shell;
using CrawlerBot.Utilities;
using Extensions.Parallelism;

namespace CrawlerBot.Crawler
{
    public class CrawlerLight : IWebCrawler
    {
        public event EventHandler<PageCrawlCompletedArgs> PageCrawlCompleted;
        public event EventHandler<PageCrawlCompletedArgs> PageCrawlCompletedAsync;
        private readonly IScheduler _scheduler;
        private readonly IThreadManager _threadManager;
        private readonly LinkParser _linkProvider;
        private readonly CrawlContext _crawlContext;
        private readonly ICrawlDecisionMaker _crawlDecisionMaker;
        private bool _crawlComplete;
        private bool _maxPagesToCrawlLimitReachedOrScheduled;
        private bool _crawlCancellationReported;
        private bool _crawlStopReported;
        private CrawlResult _crawlResult;

        public CrawlerLight() : this(null, null, null, null)
        {
        }

        public CrawlerLight(CrawlConfiguration crawlConfiguration, ICrawlDecisionMaker crawlDecisionMaker,
            IThreadManager threadManager, IScheduler scheduler)
        {
            _crawlContext = new CrawlContext
            {
                CrawlConfiguration = crawlConfiguration ?? new CrawlConfiguration()
            };

            _scheduler = scheduler ?? new Scheduler();
            int cpuBound = _crawlContext.CrawlConfiguration.MaxConcurrentThreads > 0
                ? _crawlContext.CrawlConfiguration.MaxConcurrentThreads
                : Environment.ProcessorCount;
            _threadManager = threadManager ?? new TaskThreadManager(cpuBound, _crawlContext.CancellationTokenSource);
            _linkProvider = new AngleSharpLinkParser();
            _crawlDecisionMaker = crawlDecisionMaker ?? new CrawlDirector();
            _crawlContext.Scheduler = _scheduler;
        }


        public async Task<CrawlResult> Crawl(Uri uri)
        {
            return await Crawl(uri, null);
        }

        public async Task<CrawlResult> Crawl(Uri uri, CancellationTokenSource cancellationTokenSource)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            _crawlContext.OriginalRootUri = uri;
            if (cancellationTokenSource != null)
                _crawlContext.CancellationTokenSource = cancellationTokenSource;

            _crawlResult = new CrawlResult
            {
                RootUri = _crawlContext.OriginalRootUri,
                CrawlContext = _crawlContext
            };
            var timer = Stopwatch.StartNew();
            try
            {
                var rootPage = new PageToCrawl(uri) { ParentUri = uri, IsInternal = true, IsRoot = true };
                _scheduler.Add(rootPage);
                await CrawlSite().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                _crawlResult.ErrorException = e;
            }
            finally
            {
                _threadManager?.Dispose();
            }
            timer.Stop();
            _crawlResult.Elapsed = timer.Elapsed;
            return _crawlResult;
        }

        async Task CrawlSite()
        {
            while (!_crawlComplete)
            {
                RunPreWorkChecks();
                if (_scheduler.Count > 0)
                {
                    var temp = _scheduler.GetNext();
                    await _threadManager.DoWork(async () =>
                    {
                        await ProcessPage(temp).ConfigureAwait(false);
                    }).ConfigureAwait(false);
                }
                else if (!_threadManager.HasRunningThreads())
                {
                    _crawlComplete = true;
                }
                else
                {
                    await Task.Delay(3500).ConfigureAwait(false);
                }
            }
        }

        protected virtual void RunPreWorkChecks()
        {
            CheckForCancellationRequest();
            CheckForHardStopRequest();
            CheckForStopRequest();
        }

        protected virtual void CheckForCancellationRequest()
        {
            if (_crawlContext.CancellationTokenSource.IsCancellationRequested)
            {
                if (!_crawlCancellationReported)
                {
                    string message = $"Crawl cancellation requested for site [{_crawlContext.OriginalRootUri}]!";
                    _crawlResult.ErrorException = new OperationCanceledException(message, _crawlContext.CancellationTokenSource.Token);
                    _crawlContext.IsCrawlHardStopRequested = true;
                    _crawlCancellationReported = true;
                }
            }
        }

        protected virtual void CheckForHardStopRequest()
        {
            if (_crawlContext.IsCrawlHardStopRequested)
            {
                if (!_crawlStopReported)
                {
                    _crawlStopReported = true;
                }

                _scheduler.Clear();
                _threadManager.AbortAll();
                _scheduler.Clear(); //to be sure nothing was scheduled since first call to clear()

                //Set all events to null so no longer raised.
                PageCrawlCompletedAsync = null;
                //PageCrawlStarting = null;
                //PageCrawlCompleted = null;
                //PageCrawlDisallowed = null;
                //PageLinksCrawlDisallowed = null;
            }
        }

        protected virtual void CheckForStopRequest()
        {
            if (_crawlContext.IsCrawlStopRequested)
            {
                if (!_crawlStopReported)
                {
                    _crawlStopReported = true;
                }
                _scheduler.Clear();
            }
        }

        private async Task ProcessPage(PageToCrawl pageToCrawl)
        {
            if (pageToCrawl == null)
                throw new ArgumentNullException(nameof(pageToCrawl));

            try
            {
                ThrowIfCancellationRequested();
                AddPageToContext(pageToCrawl);

                var crawledPage = await CrawlThePage(pageToCrawl).ConfigureAwait(false);

                ThrowIfCancellationRequested();
                bool shouldCrawlPageLinks = ShouldCrawlPageLinks(crawledPage);
                if (shouldCrawlPageLinks)
                    ParsePageLinks(crawledPage);

                ThrowIfCancellationRequested();

                OnPageCrawlCompleted(crawledPage);
                OnPageCrawlCompletedAsync(crawledPage);

                if (shouldCrawlPageLinks)
                    SchedulePageLinks(crawledPage);
            }
            catch (OperationCanceledException oce)
            {
                Console.WriteLine($"Thread cancelled while crawling/processing page [{pageToCrawl.Uri}]");
                throw;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error occurred during processing of page [{pageToCrawl.Uri}]");
                _crawlResult.ErrorException = exception;
                _crawlContext.IsCrawlHardStopRequested = true;
            }
        }

        protected virtual void OnPageCrawlCompleted(CrawledPage page)
        {
            PageCrawlCompleted.SafeInvoke(this, new PageCrawlCompletedArgs(_crawlContext, page));
        }
        protected virtual void OnPageCrawlCompletedAsync(CrawledPage page)
        {
            PageCrawlCompletedAsync.SafeInvokeAsync(this, new PageCrawlCompletedArgs(_crawlContext, page));
        }

        protected virtual void AddPageToContext(PageToCrawl pageToCrawl)
        {
            //if (pageToCrawl.IsRetry)
            //{
            //    pageToCrawl.RetryCount++;
            //    return;
            //}

            Interlocked.Increment(ref _crawlContext.CrawledCount);
            _crawlContext.CrawlCountByDomain.AddOrUpdate(pageToCrawl.Uri.Authority, 1, (key, oldValue) => oldValue + 1);
        }
        protected virtual void ThrowIfCancellationRequested()
        {
            if (_crawlContext.CancellationTokenSource != null && _crawlContext.CancellationTokenSource.IsCancellationRequested)
                _crawlContext.CancellationTokenSource.Token.ThrowIfCancellationRequested();
        }

        protected virtual void SchedulePageLinks(CrawledPage crawledPage)
        {
            int linksToCrawl = 0;
            foreach (var uri in crawledPage.ParsedLinks)
            {
                if (!_scheduler.IsUriKnown(uri))
                {
                    try
                    {
                        var page = new PageToCrawl(uri)
                        {
                            ParentUri = crawledPage.Uri,
                            CrawlDepth = crawledPage.CrawlDepth + 1,
                            IsInternal = IsInternalUri(uri),
                            IsRoot = false
                        };

                        if (ShouldSchedulePageLink(page))
                        {
                            _scheduler.Add(page);
                            linksToCrawl++;
                        }

                        if (!ShouldScheduleMorePageLink(linksToCrawl))
                            break;
                    }
                    catch (Exception e)
                    {
                        // ignored 
                    }
                }
            }
        }

        protected virtual bool ShouldSchedulePageLink(PageToCrawl page)
        {
            if ((page.IsInternal || _crawlContext.CrawlConfiguration.IsExternalPageCrawlingEnabled) && (ShouldCrawlPage(page)))
                return true;

            return false;
        }

        protected virtual bool ShouldCrawlPageLinks(CrawledPage crawledPage)
        {
            CrawlDecision shouldCrawlPageLinksDecision = _crawlDecisionMaker.ShouldCrawlPageLinks(crawledPage, _crawlContext);

            //if (shouldCrawlPageLinksDecision.Allow)
            //    shouldCrawlPageLinksDecision = (_shouldCrawlPageLinksDecisionMaker != null) ? _shouldCrawlPageLinksDecisionMaker.Invoke(crawledPage, _crawlContext) : new CrawlDecision { Allow = true };
            //if (!shouldCrawlPageLinksDecision.Allow)
            //{
            //    _logger.DebugFormat("Links on page [{0}] not crawled, [{1}]", crawledPage.Uri.AbsoluteUri, shouldCrawlPageLinksDecision.Reason);
            //    FirePageLinksCrawlDisallowedEventAsync(crawledPage, shouldCrawlPageLinksDecision.Reason);
            //    FirePageLinksCrawlDisallowedEvent(crawledPage, shouldCrawlPageLinksDecision.Reason);
            //}

            SignalCrawlStopIfNeeded(shouldCrawlPageLinksDecision);
            return shouldCrawlPageLinksDecision.Allow;
        }

        protected virtual bool ShouldCrawlPage(PageToCrawl pageToCrawl)
        {
            if (_maxPagesToCrawlLimitReachedOrScheduled)
                return false;

            var shouldCrawlPageDecision = _crawlDecisionMaker.ShouldCrawlPage(pageToCrawl, _crawlContext);
            if (!shouldCrawlPageDecision.Allow &&
                shouldCrawlPageDecision.Reason.Contains("MaxPagesToCrawl limit of"))
            {
                _maxPagesToCrawlLimitReachedOrScheduled = true;
                return false;
            }

            //if (shouldCrawlPageDecision.Allow)
            //    shouldCrawlPageDecision = (_shouldCrawlPageDecisionMaker != null) ? _shouldCrawlPageDecisionMaker.Invoke(pageToCrawl, _crawlContext) : new CrawlDecision { Allow = true };
            //if (!shouldCrawlPageDecision.Allow)
            //{
            //    _logger.DebugFormat("Page [{0}] not crawled, [{1}]", pageToCrawl.Uri.AbsoluteUri, shouldCrawlPageDecision.Reason);
            //    FirePageCrawlDisallowedEventAsync(pageToCrawl, shouldCrawlPageDecision.Reason);
            //    FirePageCrawlDisallowedEvent(pageToCrawl, shouldCrawlPageDecision.Reason);
            //}

            SignalCrawlStopIfNeeded(shouldCrawlPageDecision);
            return shouldCrawlPageDecision.Allow;
        }

        protected virtual void SignalCrawlStopIfNeeded(CrawlDecision decision)
        {
            if (decision.ShouldHardStopCrawl)
            {
                _crawlContext.IsCrawlHardStopRequested = decision.ShouldHardStopCrawl;
            }
            else if (decision.ShouldStopCrawl)
            {
                _crawlContext.IsCrawlStopRequested = decision.ShouldStopCrawl;
            }
        }

        protected virtual bool ShouldScheduleMorePageLink(int linksAdded)
        {
            return _crawlContext.CrawlConfiguration.MaxLinksPerPage == 0 || _crawlContext.CrawlConfiguration.MaxLinksPerPage > linksAdded;
        }

        protected virtual CrawlDecision ShouldDownloadPageContent(CrawledPage crawledPage)
        {
            var decision = _crawlDecisionMaker.ShouldDownloadPageContent(crawledPage, _crawlContext);
            SignalCrawlStopIfNeeded(decision);
            return decision;
        }

        protected virtual bool IsInternalUri(Uri uri)
        {
            return uri.Authority == _crawlContext.OriginalRootUri.Authority;
        }

        protected virtual void ParsePageLinks(CrawledPage crawledPage)
        {
            crawledPage.ParsedLinks = _linkProvider.GetLinks(crawledPage);
        }

        private async Task<CrawledPage> CrawlThePage(PageToCrawl pageToCrawl)
        {
            var crawledPage = await pageToCrawl.MakeRequest(ShouldDownloadPageContent).ConfigureAwait(false);
            Map(pageToCrawl, crawledPage);
            return crawledPage;
        }

        protected virtual void Map(PageToCrawl pageToCrawl, CrawledPage crawledPage)
        {
            crawledPage.IsRoot = pageToCrawl.IsRoot;
            crawledPage.IsInternal = pageToCrawl.IsInternal;
            crawledPage.CrawlDepth = pageToCrawl.CrawlDepth;
            crawledPage.ParentUri = pageToCrawl.ParentUri;
        }
    }
}
