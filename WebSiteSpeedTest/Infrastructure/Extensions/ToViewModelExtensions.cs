using UtilitiesPackage;
using WebSiteSpeedTest.Models;

namespace WebSiteSpeedTest.Infrastructure.Extensions
{
    public static class ToViewModelExtensions
    {
        public static MeasurementResultViewModel ToViewModel(this MeasurementResult input)
        {
            return new MeasurementResultViewModel(input.Url, $"{input.MinTime.TotalSeconds:N2}", $"{input.MaxTime.TotalSeconds:N2}");
        }
    }
}