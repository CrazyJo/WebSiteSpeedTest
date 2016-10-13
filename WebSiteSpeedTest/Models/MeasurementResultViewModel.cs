namespace WebSiteSpeedTest.Models
{
    public struct MeasurementResultViewModel
    {
        public string time;
        public string url;

        public MeasurementResultViewModel(string url, string time)
        {
            this.url = url;
            this.time = time;
        }
    }
}