namespace WebSiteSpeedTest.Models
{
    public struct MeasurementResultViewModel
    {
        public string mintime;
        public string maxtime;
        public string url;

        public MeasurementResultViewModel(string url, string mintime, string maxtime)
        {
            this.url = url;
            this.mintime = mintime;
            this.maxtime = maxtime;
        }

    }
}