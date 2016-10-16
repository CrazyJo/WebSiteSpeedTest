using System;
using System.Collections.Concurrent;

namespace WebSiteSpeedTest.Models.Db.Entities
{
    public class HistoryRow : MeasurementResult
    {
        public DateTime Date { get; set; }

        public virtual ConcurrentBag<SitemapRow> SitemapRows { get; set; }

        public HistoryRow()
        {
            SitemapRows = new ConcurrentBag<SitemapRow>();
        }
    }
}