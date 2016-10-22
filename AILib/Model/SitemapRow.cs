namespace Core.Model
{
    public class SitemapRow : MeasurementResult
    {
        public int Id { get; set; }

        public string HistoryRowId { get; set; }

        public virtual HistoryRow HistoryRow { get; set; }
    }
}