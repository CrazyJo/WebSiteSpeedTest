using System;

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

        public string Url { get; set; }
        public TimeSpan MinTime { get; set; }
        public TimeSpan MaxTime { get; set; }

        public override string ToString()
        {
            return $"{MinTime.TotalSeconds:N2}, {MaxTime.TotalSeconds:N2}";
        }
    }
}