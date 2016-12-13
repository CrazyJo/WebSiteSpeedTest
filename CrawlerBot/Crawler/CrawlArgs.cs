using System;
using CrawlerBot.Shell;

namespace CrawlerBot.Crawler
{
    public class CrawlArgs: EventArgs
    {
        public CrawlContext CrawlContext { get; set; }

        public CrawlArgs(CrawlContext crawlContext)
        {
            if (crawlContext == null)
                throw new ArgumentNullException(nameof(crawlContext));

            CrawlContext = crawlContext;
        }
    }
}
