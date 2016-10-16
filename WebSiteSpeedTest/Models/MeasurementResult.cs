using System;

namespace WebSiteSpeedTest.Models
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
        public int Id { get; set; }
        public string Url { get; set; }
        public TimeSpan MinTime { get; set; }
        public TimeSpan MaxTime { get; set; }
    }
}
