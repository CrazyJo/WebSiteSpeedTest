using System;
using Newtonsoft.Json;

namespace Core.Model
{
    public class MeasurementResult
    {
        public MeasurementResult()
        {
        }

        public MeasurementResult(string url, TimeSpan minTime, TimeSpan maxTime)
        {
            Url = url;
            MinTime = minTime;
            MaxTime = maxTime;
        }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("mintime")]
        public TimeSpan MinTime { get; set; }

        [JsonProperty("maxtime")]
        public TimeSpan MaxTime { get; set; }
    }
}