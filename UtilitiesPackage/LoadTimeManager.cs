using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AILib;

namespace UtilitiesPackage
{
    public class LoadTimeManager : ILoadTimeManager
    {
        public IDictionary<string, TimeSpan> LoadTimeMeasuringWithSitemap(string url)
        {
            Dictionary<string, TimeSpan> results = new Dictionary<string, TimeSpan>();

            var sitemapLinks = ParseSitemap(url);

            results.Add(url, LoadTimeMeasuring(url));

            foreach (var link in sitemapLinks)
            {
                results.Add(link, LoadTimeMeasuring(link));
            }

            return results;
        }

        public Task<IDictionary<string, TimeSpan>> LoadTimeMeasuringWithSitemapAsync(string url)
        {
            var d = new Func<string, IDictionary<string, TimeSpan>>(LoadTimeMeasuringWithSitemap);

            return Task.Run(() => d.Invoke(url));
        }

        public virtual TimeSpan LoadTimeMeasuring(string url)
        {
            var sw = new Stopwatch();

            using (var client = new System.Net.Http.HttpClient())
            {
                sw.Start();
                client.GetAsync(url);
                sw.Stop();
            }
            return sw.Elapsed;
        }

        public virtual IEnumerable<string> ParseSitemap(string url)
        {
            return SitemapWorker.ParseSitemapFile(url.GetDomain() + "/sitemap.xml");
        }
    }
}
