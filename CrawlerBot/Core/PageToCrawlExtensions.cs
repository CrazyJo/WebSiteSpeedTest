using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using CrawlerBot.Shell;

namespace CrawlerBot.Core
{
    public static class PageToCrawlExtensions
    {
        public static async Task<CrawledPage> MakeRequest(this PageToCrawl pageToCrawl, Func<CrawledPage, CrawlDecision> shouldDownloadContent)
        {
            var http = new HttpClient();
            var crawledPage = new CrawledPage(pageToCrawl.Uri);

            var stopwatch = Stopwatch.StartNew();
            var response = await http.GetAsync(pageToCrawl.Uri).ConfigureAwait(false);

            stopwatch.Stop();
            crawledPage.Elapsed = stopwatch.Elapsed;

            crawledPage.HttpWebResponse = response;
            var decision = shouldDownloadContent(crawledPage);
            if (decision.Allow)
            {
                var temp = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                crawledPage.RawContent = temp;
            }
            return crawledPage;
        }
    }
}
