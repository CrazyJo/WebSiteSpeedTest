using System.Collections.Generic;
using Core.Model;

namespace Data
{
    public static class Committer
    {
        private static readonly Repo<HistoryRow> HistoryRepo;
        private static readonly Repo<SitemapRow> SmRepo;

        static Committer()
        {
            IDbContextFactory db = new DbContextFactory();
            HistoryRepo  = new Repo<HistoryRow>(db);
            SmRepo = new Repo<SitemapRow>(db);
        }

        public static void Save(HistoryRow historyRow, IEnumerable<SitemapRow> sitemapRows)
        {
            SaveHistory(historyRow);
            SaveSitemap(sitemapRows);
        }

        public static void SaveHistory(HistoryRow historyRow)
        {
            HistoryRepo.Insert(historyRow);
        }

        public static void SaveSitemap(IEnumerable<SitemapRow> sitemapRows)
        {
            SmRepo.AddRange(sitemapRows);
        }
    }
}
