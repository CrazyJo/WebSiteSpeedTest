using CrawlerBot.Shell;

namespace UtilitiesPackage.WebtesterPack
{
    public class PageTestRefusedArg : TestArg
    {
        public CrawledPage CrawledPage { get; set; }

        public PageTestRefusedArg(CrawledPage crawledPage, WebTestContext context) : base(context)
        {
            CrawledPage = crawledPage;
        }
    }
}
