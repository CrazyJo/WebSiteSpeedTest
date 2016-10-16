using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSiteSpeedTest.Models.Db.Entities
{
    public class SitemapRow : MeasurementResult
    {
        public int HistoryRowId { get; set; }

        public virtual HistoryRow HistoryRow { get; set; }
    }
}