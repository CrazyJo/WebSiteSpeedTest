using System;

namespace UtilitiesPackage
{
    public struct MeasurementResult
    {
        public MeasurementResult(string url, TimeSpan minTime, TimeSpan maxTime)
        {
            Url = url;
            MinTime = minTime;
            MaxTime = maxTime;
        }

        public string Url { get; set; }
        public TimeSpan MinTime { get; set; }
        public TimeSpan MaxTime { get; set; }
    }
}
