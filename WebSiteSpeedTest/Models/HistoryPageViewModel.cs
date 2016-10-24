using System.Collections.Generic;

namespace WebSiteSpeedTest.Models
{
    public class HistoryPageViewModel<T>
    {
        public IEnumerable<T> Content { get; set; }
        public HistoryPagerViewModel HistoryPager { get; set; }
    }
}