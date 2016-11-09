using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Model
{
    public class HistoryRow : MeasurementResult
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<SitemapRow> SitemapRows { get; set; }
    }
}