using System.Collections.Generic;

namespace WebSiteSpeedTest.Models
{
    public class HistoryPageViewModel<T>
    {
        public IEnumerable<T> Content { get; set; }
        public bool IsLastPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool PaginationEnable { get; set; }
    }
}