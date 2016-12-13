using System;
using System.Collections.Concurrent;

namespace CrawlerBot.Core
{

    public class InMemoryCrawledUrlStorage : ICrawledUrlRepository
    {
        readonly ConcurrentDictionary<string, byte> _urlRepository = new ConcurrentDictionary<string, byte>(); 

        public bool AddIfNew(Uri uri)
        {
            return _urlRepository.TryAdd(uri.AbsoluteUri, byte.MinValue);
        }

        public bool Contains(Uri uri)
        {
            return _urlRepository.ContainsKey(uri.AbsoluteUri);
        }
    }
}
