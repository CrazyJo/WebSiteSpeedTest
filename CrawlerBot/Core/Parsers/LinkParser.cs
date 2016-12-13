using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CrawlerBot.Shell;

namespace CrawlerBot.Core.Parsers
{
    public abstract class LinkParser
    {
        /// <returns>It returns the unique values.</returns>
        public IEnumerable<Uri> GetLinks(CrawledPage crawledPage)
        {
            var hrefs = GetHrefValues(crawledPage);
            var res = GetUris(crawledPage, hrefs);
            return res;
        }

        protected abstract IEnumerable<string> GetHrefValues(CrawledPage crawledPage);

        protected virtual IEnumerable<Uri> GetUris(CrawledPage crawledPage, IEnumerable<string> hrefValues)
        {
            var uris = new HashSet<Uri>();
            var enumerable = hrefValues as IList<string> ?? hrefValues.ToList();
            if (!enumerable.Any())
                return uris;

            if (crawledPage == null)
                throw new ArgumentNullException(nameof(crawledPage));

            Uri uriToUse = crawledPage.HttpWebResponse.RequestMessage.RequestUri ?? crawledPage.Uri;
            foreach (var value in enumerable)
            {
                try
                {
                    var uri = new Uri(uriToUse, value);
                    uris.Add(uri);

                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            return uris;
        }

        private async Task<string> GetPageAsString(Uri uri, int timeout = 7500)
        {
            var http = new HttpClient();
            var task = http.GetStringAsync(uri);
            var t = await Task.WhenAny(task, Task.Delay(timeout));
            if (t == task && task.Status == TaskStatus.RanToCompletion)
            {
                return task.Result;
            }
            throw new TimeoutException();
        }
    }
}