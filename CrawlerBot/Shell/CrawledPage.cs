using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CrawlerBot.Shell
{
    public class CrawledPage : PageToCrawl
    {
        /// <summary>
        /// Server response time
        /// </summary>
        public TimeSpan Elapsed { get; set; }

        /// <summary>
        /// Web response from the server.
        /// </summary>
        public HttpResponseMessage HttpWebResponse { get; set; }

        public IEnumerable<Uri> ParsedLinks { get; set; }
        public string RawContent { get; set; }

        public CrawledPage(Uri uri) : base(uri)
        {
        }
    }
}
