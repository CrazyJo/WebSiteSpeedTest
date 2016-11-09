using System.Collections.Generic;
using System.Linq;
using Core.Model;
using WebSiteSpeedTest.Models;

namespace WebSiteSpeedTest.Infrastructure.Extensions
{
    public static class ToViewModelExtensions
    {
        public static MeasurementResultViewModel ToViewModel(this MeasurementResult input)
        {
            return new MeasurementResultViewModel(input.Url, input.MinTime.TotalSeconds, input.MaxTime.TotalSeconds);
        }

        public static HistoryPageViewModel<IEnumerable<T>> GetHistoryPage<T>(this IList<T> source, int startIndex, int pageCapacity, string callBackUrl, string historyRowId = null)
        {
            var rowsCount = source.Count;
            var enablePagination = pageCapacity < rowsCount;

            var res = new HistoryPageViewModel<IEnumerable<T>>
            {
                Content = source.Take(pageCapacity),
                HistoryPager = new HistoryPagerViewModel
                {
                    EnablePagination = enablePagination,
                    IsLastPage = !enablePagination,
                    IsFirstPage = startIndex == 0,
                    NextStartIndex = startIndex + pageCapacity,
                    PreviousStartIndex = startIndex - pageCapacity,
                    ActionUrl = callBackUrl,
                    HistoryRowId = historyRowId
                }
            };

            return res;
        }
    }
}