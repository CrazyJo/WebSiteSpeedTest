using System;

namespace UtilitiesPackage
{
    public struct MeasurementResult
    {
        public MeasurementResult(string url, TimeSpan time)
        {
            Url = url;
            Time = time;
        }

        public string Url { get; set; }
        public TimeSpan Time { get; set; }
    }
}
