using System;

namespace CrawlerBot.Shell
{
    public class PageToCrawl
    {
        public Uri Uri { get; set; }
        public int CrawlDepth { get; set; }
        public bool IsRoot { get; set; }
        public bool IsInternal { get; set; }
        public Uri ParentUri { get; set; }

        public PageToCrawl(Uri uri)
        {
            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            Uri = uri;
        }
    }
}