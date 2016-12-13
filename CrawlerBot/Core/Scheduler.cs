using System;
using System.Collections.Generic;
using CrawlerBot.Shell;

namespace CrawlerBot.Core
{
    public class Scheduler : IScheduler
    {
        readonly ICrawledUrlRepository _crawledUrlRepo;
        readonly IPagesToCrawlRepository _pagesToCrawlRepo;

        public Scheduler()
            : this(null, null)
        {
        }

        public Scheduler(ICrawledUrlRepository crawledUrlRepo, IPagesToCrawlRepository pagesToCrawlRepo)
        {
            _crawledUrlRepo = crawledUrlRepo ?? new InMemoryCrawledUrlStorage();
            _pagesToCrawlRepo = pagesToCrawlRepo ?? new FifoPagesToCrawlStorage();
        }

        public int Count => _pagesToCrawlRepo.Count();

        public void Add(PageToCrawl page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (_crawledUrlRepo.AddIfNew(page.Uri))
                _pagesToCrawlRepo.Add(page);
        }

        public void Add(IEnumerable<PageToCrawl> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            foreach (PageToCrawl page in pages)
                Add(page);
        }

        public PageToCrawl GetNext()
        {
            return _pagesToCrawlRepo.GetNext();
        }

        public void Clear()
        {
            _pagesToCrawlRepo.Clear();
        }

        public void AddKnownUri(Uri uri)
        {
            _crawledUrlRepo.AddIfNew(uri);
        }

        public bool IsUriKnown(Uri uri)
        {
            return _crawledUrlRepo.Contains(uri);
        }
    }
}
