namespace CrawlerBot.Shell
{
    public class CrawlConfiguration
    {
        public CrawlConfiguration()
        {
            MaxConcurrentThreads = 10;
            MaxPagesToCrawl = 5;
            MaxCrawlDepth = 1000;
        }

        public bool CrawlStaticFiles { get; set; }
        public int MaxCrawlDepth { get; set; }
        public int MaxLinksPerPage { get; set; }
        public int MaxConcurrentThreads { get; set; }
        public int MaxPagesToCrawlPerDomain { get; set; }

        /// <summary>
        /// Whether pages external to the root uri should be crawled
        /// </summary>
        public bool IsExternalPageCrawlingEnabled { get; set; }

        /// <summary>
        /// Maximum number of pages to crawl. 
        /// If zero, this setting has no effect
        /// </summary>
        public int MaxPagesToCrawl { get; set; }

        /// <summary>
        /// Whether pages external to the root uri should have their links crawled. NOTE: IsExternalPageCrawlEnabled must be true for this setting to have any effect
        /// </summary>
        public bool IsExternalPageLinksCrawlingEnabled { get; set; }
    }
}
