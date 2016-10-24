namespace WebSiteSpeedTest.Models
{
    public class HistoryPagerViewModel
    {
        public bool IsLastPage { get; set; }
        public bool IsFirstPage { get; set; }
        public bool EnablePagination { get; set; }
        public int NextStartIndex { get; set; }
        public int PreviousStartIndex { get; set; }
        public string ActionUrl { get; set; }
    }
}