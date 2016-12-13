using System;
using CrawlerBot.Shell;

namespace CrawlerBot.Crawler
{
    public class PageCrawlCompletedArgs : CrawlArgs
    {
        public CrawledPage CrawledPage { get; private set; }

        public PageCrawlCompletedArgs(CrawlContext crawlContext, CrawledPage crawledPage)
            : base(crawlContext)
        {
            if (crawledPage == null)
                throw new ArgumentNullException(nameof(crawledPage));

            CrawledPage = crawledPage;
        }
    }
}
