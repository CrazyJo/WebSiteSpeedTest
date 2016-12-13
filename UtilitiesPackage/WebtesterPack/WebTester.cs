using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using CrawlerBot.Core;
using CrawlerBot.Crawler;
using CrawlerBot.Shell;
using Extensions.Parallelism;

namespace UtilitiesPackage.WebtesterPack
{
    public class WebTester : IWebTester
    {
        public event EventHandler<PageTestRefusedArg> PageTestRefusedAsync;
        public event EventHandler<PageTestedArg> PageTestedAsync;
        private BufferBlock<CrawledPage> _executionQueueBlock;
        private TransformBlock<CrawledPage, PageTestedResult> _testBlock;
        private ActionBlock<PageTestedResult> _notificationBlock;
        private readonly IWebCrawler _crawler;
         WebTestContext Context;

        public WebTester(CancellationTokenSource tokenSource, TestOptions options)
        {
            Context = new WebTestContext
            {
                TestOptions = options ?? new TestOptions(),
                CancellationTokenSource = tokenSource ?? new CancellationTokenSource()
            };
            BlockInit();
            _crawler = new CrawlerLight();
            _crawler.PageCrawlCompletedAsync += _crawler_PageCrawlCompleted;
        }

        void BlockInit()
        {
            var executionblockOptions = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = Context.TestOptions.MaxDegreeOfParallelism
            };
            var dataflowBlockOptions = new DataflowBlockOptions
            {
                CancellationToken = Context.CancellationTokenSource.Token
            };

            _executionQueueBlock = new BufferBlock<CrawledPage>(dataflowBlockOptions);
            _testBlock =
                new TransformBlock<CrawledPage, PageTestedResult>(new Func<CrawledPage, Task<PageTestedResult>>(TestPage),
                    executionblockOptions);
            _notificationBlock = new ActionBlock<PageTestedResult>(new Action<PageTestedResult>(PageTestСompleted),
                executionblockOptions);

            _executionQueueBlock.LinkTo(_testBlock);
            _testBlock.LinkTo(_notificationBlock);

            _executionQueueBlock.Completion.ContinueWith(t =>
            {
                if (t.IsFaulted) ((IDataflowBlock)_testBlock).Fault(t.Exception);
                else _testBlock.Complete();
            });
            _testBlock.Completion.ContinueWith(t =>
            {
                if (t.IsFaulted) ((IDataflowBlock)_notificationBlock).Fault(t.Exception);
                else _notificationBlock.Complete();
            });
        }

        public async Task<WebTestResult> BeginTest(Uri uri)
        {
            Context.TestedPagesCount = 0;
            WebTestResult testResult = new WebTestResult();
            CrawlResult crawlRes = null;
            var stopwatch = Stopwatch.StartNew();
            try
            {
                crawlRes = await CrawlAndTest(uri).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                testResult.Exception = e;
            }
            finally
            {
                stopwatch.Stop();
                testResult.Elapsed = stopwatch.Elapsed;
            }
            MapResult(testResult, crawlRes);
            return testResult;
        }

         void MapResult(WebTestResult testResult, CrawlResult crawlRes)
        {
            if (crawlRes == null || testResult == null) return;
            int crawledCount;
            crawlRes.CrawlContext.CrawlCountByDomain.TryGetValue(crawlRes.RootUri.Authority, out crawledCount);
            testResult.CrawledPagesCount = crawledCount;
            testResult.TestedPagesCount = Context.TestedPagesCount;
            testResult.Exception = crawlRes.ErrorException;
        }

         async Task<CrawlResult> CrawlAndTest(Uri uri)
        {
            var res = await _crawler.Crawl(uri, Context.CancellationTokenSource).ConfigureAwait(false);
            _executionQueueBlock.Complete();
            await _notificationBlock.Completion.ConfigureAwait(false);
            return res;
        }

        void PageTestСompleted(PageTestedResult res)
        {
            IncreaseTestedPageCount();
            OnPageTested(res);
        }

        async Task<PageTestedResult> TestPage(CrawledPage page)
        {
            var res = new PageTestedResult(page);
            var resList = await LoadStopwatch.LoadSeveralTimes(page.Uri, Context.TestOptions.PageReloadNumber, Context.CancellationTokenSource.Token).ConfigureAwait(false);
            res.Measurements = resList;
            return res;
        }

         void IncreaseTestedPageCount()
        {
            Interlocked.Increment(ref Context.TestedPagesCount);
        }

          void ScheduleTest(CrawledPage page)
        {
            _executionQueueBlock.Post(page);
        }

         void _crawler_PageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            if (ShouldTestCrawledPage(e.CrawledPage))
            {
                ScheduleTest(e.CrawledPage);
            }
            else
            {
                OnPageTestRefusedAsync(e.CrawledPage);
            }
        }

          bool ShouldTestCrawledPage(CrawledPage page)
        {
            if (page == null) return false;
            if (Context.CancellationTokenSource.IsCancellationRequested) return false;
            return page.HttpWebResponse.IsSuccessStatusCode;
        }

        protected virtual void OnPageTested(PageTestedResult result)
        {
            PageTestedAsync.SafeInvoke(this, new PageTestedArg(result, Context));
        }

        protected virtual void OnPageTestRefusedAsync(CrawledPage page)
        {
            PageTestRefusedAsync.SafeInvokeAsync(this, new PageTestRefusedArg(page, Context));
        }
    }
}
