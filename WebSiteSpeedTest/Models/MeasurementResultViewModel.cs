namespace WebSiteSpeedTest.Models
{
    public struct MeasurementResultViewModel
    {
        public string url;
        public double mintime;
        public double maxtime;

        public MeasurementResultViewModel(string url, double mintime, double maxtime)
        {
            this.url = url;
            this.mintime = mintime;
            this.maxtime = maxtime;
        }
    }
}