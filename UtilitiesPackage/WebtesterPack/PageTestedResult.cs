using System;
using System.Collections.Generic;
using CrawlerBot.Shell;

namespace UtilitiesPackage.WebtesterPack
{
    public class PageTestedResult
    {
        public PageTestedResult(CrawledPage crawledPage)
        {
            CrawledPage = crawledPage;
        }
        public CrawledPage CrawledPage { get; set; }
        public IEnumerable<TimeSpan> Measurements { get; set; }
    }
}
