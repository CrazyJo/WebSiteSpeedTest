using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CrawlerBot.Crawler;
using CrawlerBot.Shell;

namespace CrawlerBot.Core
{
    public interface IWebCrawler
    {
        event EventHandler<PageCrawlCompletedArgs> PageCrawlCompleted;
        event EventHandler<PageCrawlCompletedArgs> PageCrawlCompletedAsync;

        /// <summary>
        /// Begins a crawl using the uri param
        /// </summary>
        Task<CrawlResult> Crawl(Uri uri);

        /// <summary>
        /// Begins a crawl using the uri param, and can be cancelled using the CancellationToken
        /// </summary>
        Task<CrawlResult> Crawl(Uri uri, CancellationTokenSource tokenSource);
    }

    public interface IPagesToCrawlRepository
    {
        void Add(PageToCrawl page);
        PageToCrawl GetNext();
        void Clear();
        int Count();

    }

    public interface ICrawledUrlRepository
    {
        bool Contains(Uri uri);
        bool AddIfNew(Uri uri);
    }

    public interface ICrawlDecisionMaker
    {
        /// <summary>
        /// Decides whether the page should be crawled
        /// </summary>
        CrawlDecision ShouldCrawlPage(PageToCrawl pageToCrawl, CrawlContext crawlContext);

        /// <summary>
        /// Decides whether the page's links should be crawled
        /// </summary>
        CrawlDecision ShouldCrawlPageLinks(CrawledPage crawledPage, CrawlContext crawlContext);

        /// <summary>
        /// Decides whether the page's content should be dowloaded
        /// </summary>
        CrawlDecision ShouldDownloadPageContent(CrawledPage crawledPage, CrawlContext crawlContext);
    }


    public interface IScheduler
    {
        /// <summary>
        /// Count of remaining items that are currently scheduled
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Schedules the param to be crawled
        /// </summary>
        void Add(PageToCrawl page);

        /// <summary>
        /// Schedules the param to be crawled
        /// </summary>
        void Add(IEnumerable<PageToCrawl> pages);

        /// <summary>
        /// Gets the next page to crawl
        /// </summary>
        PageToCrawl GetNext();

        /// <summary>
        /// Clear all currently scheduled pages
        /// </summary>
        void Clear();

        /// <summary>
        /// Add the Url to the list of crawled Url without scheduling it to be crawled.
        /// </summary>
        /// <param name="uri"></param>
        void AddKnownUri(Uri uri);

        /// <summary>
        /// Returns whether or not the specified Uri was already scheduled to be crawled or simply added to the
        /// list of known Uris.
        /// </summary>
        bool IsUriKnown(Uri uri);
    }

    public interface IThreadManager : IDisposable
    {
        /// <summary>
        /// Max number of threads to use.
        /// </summary>
        int MaxThreads { get; set; }

        /// <summary>
        /// Will perform the action asynchrously on a seperate thread
        /// </summary>
        /// <param name="action">The action to perform</param>
        Task DoWork(Func<Task> action);

        /// <summary>
        /// Whether there are running threads
        /// </summary>
        bool HasRunningThreads();

        /// <summary>
        /// Abort all running threads
        /// </summary>
        void AbortAll();
    }
}
