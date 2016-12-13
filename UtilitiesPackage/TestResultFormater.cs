using System;
using System.Threading;
using Core.Model;
using Extensions.Enumerable;
using UtilitiesPackage.WebtesterPack;

namespace UtilitiesPackage
{
    public class TestResultFormater : IFormater<PageTestedResult, MeasurementResult>
    {
        private string _currentId;
        Lazy<string> currId = new Lazy<string>(() => new Guid().ToString(), true);

        protected string CurrentId
        {
            get
            {
                LazyInitializer.EnsureInitialized(ref _currentId, () => Guid.NewGuid().ToString());
                return _currentId;
            }
            set
            {
                Volatile.Write(ref _currentId, value);
            }
        }

        public MeasurementResult Format(PageTestedResult testedResult)
        {
            if (testedResult.CrawledPage.IsRoot)
            {
                return ToHistoryRow(testedResult);
            }
            return ToSitemapRow(testedResult);
        }

        HistoryRow ToHistoryRow(PageTestedResult testedResult)
        {
            var hr = new HistoryRow
            {
                Id = CurrentId,
                Date = DateTime.UtcNow
            };
            Map(hr, testedResult);
            return hr;
        }

        SitemapRow ToSitemapRow(PageTestedResult testedResult)
        {
            var sr = new SitemapRow
            {
                HistoryRowId = CurrentId
            };
            Map(sr, testedResult);
            return sr;
        }

        void Map(MeasurementResult mR, PageTestedResult tR)
        {
            mR.Url = tR.CrawledPage.Uri.AbsoluteUri;
            mR.MaxTime = tR.Measurements.GetMax(tR.CrawledPage.Elapsed);
            mR.MinTime = tR.Measurements.GetMin(tR.CrawledPage.Elapsed);
        }

        public void Dispose()
        {
            CurrentId = null;
        }
    }
}
