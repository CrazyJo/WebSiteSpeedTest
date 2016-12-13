using System;
using System.Collections.Concurrent;
using System.Threading;
using CrawlerBot.Core;

namespace CrawlerBot.Shell
{
    public class CrawlContext
    {
        public CrawlContext()
        {
            CrawlCountByDomain = new ConcurrentDictionary<string, int>();
            CancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Total number of pages that have been crawled
        /// </summary>
        public int CrawledCount = 0;

        /// <summary>
        /// The root of the crawl specified in the configuration.
        /// </summary>
        public Uri OriginalRootUri { get; set; }

        /// <summary>
        /// Configuration values used to determine crawl settings
        /// </summary>
        public CrawlConfiguration CrawlConfiguration { get; set; }

        /// <summary>
        /// Cancellation token used to hard stop the crawl. Will clear all scheduled pages and abort any threads that are currently crawling.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>
        /// The scheduler that is being used
        /// </summary>
        public IScheduler Scheduler { get; set; }

        /// <summary>
        /// Threadsafe dictionary of domains and how many pages were crawled in that domain
        /// </summary>
        public ConcurrentDictionary<string, int> CrawlCountByDomain { get; set; }

        /// <summary>
        /// Whether a request to hard stop the crawl has happened. Will clear all scheduled pages and cancel any threads that are currently crawling.
        /// </summary>
        public bool IsCrawlHardStopRequested { get; set; }

        /// <summary>
        /// Whether a request to stop the crawl has happened. Will clear all scheduled pages but will allow any threads that are currently crawling to complete.
        /// </summary>
        public bool IsCrawlStopRequested { get; set; }

    }
}
