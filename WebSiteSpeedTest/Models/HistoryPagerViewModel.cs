using Newtonsoft.Json;

namespace WebSiteSpeedTest.Models
{
    public class HistoryPagerViewModel
    {
        [JsonProperty(PropertyName = "isLastPage")]
        public bool IsLastPage { get; set; }

        [JsonProperty(PropertyName = "isFirstPage")]
        public bool IsFirstPage { get; set; }

        [JsonIgnore]
        public bool EnablePagination { get; set; }

        [JsonProperty(PropertyName = "nextStartIndex")]
        public int NextStartIndex { get; set; }

        [JsonProperty(PropertyName = "previousStartIndex")]
        public int PreviousStartIndex { get; set; }

        [JsonIgnore]
        public string ActionUrl { get; set; }

        [JsonIgnore]
        public string HistoryRowId { get; set; }
    }
}