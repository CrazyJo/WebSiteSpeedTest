using System.Collections.Generic;

namespace Core.Model
{
    public class ResultsPack
    {
        public HistoryRow HistoryRow;
        public IEnumerable<SitemapRow> SitemapRows;

        public ResultsPack(HistoryRow historyRow, IEnumerable<SitemapRow> sitemapRows)
        {
            HistoryRow = historyRow;
            SitemapRows = sitemapRows;
        }
    }
}
