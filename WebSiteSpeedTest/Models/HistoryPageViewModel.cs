using Newtonsoft.Json;

namespace WebSiteSpeedTest.Models
{
    public class HistoryPageViewModel<T>
    {
        [JsonProperty(PropertyName = "contentHistory")]
        public T Content { get; set; }

        [JsonProperty(PropertyName = "historyPager")]
        public HistoryPagerViewModel HistoryPager { get; set; }
    }
}