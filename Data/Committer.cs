using System;
using System.Collections.Generic;
using System.Linq;
using Core.Model;

namespace Data
{
    public class Committer : IDisposable
    {
        private readonly Repo<HistoryRow> _historyRepo;
        private readonly Repo<SitemapRow> _smRepo;

        public Committer()
        {
            IDbContextFactory db = new DbContextFactory();
            _historyRepo  = new Repo<HistoryRow>(db);
            _smRepo = new Repo<SitemapRow>(db);
        }

        public void Save(HistoryRow historyRow, IEnumerable<SitemapRow> sitemapRows)
        {
            SaveHistory(historyRow);
            SaveSitemap(sitemapRows);
        }

        public void SaveHistory(HistoryRow historyRow)
        {
            _historyRepo.Insert(historyRow);
        }

        public void SaveSitemap(IEnumerable<SitemapRow> sitemapRows)
        {
            _smRepo.AddRange(sitemapRows);
        }

        public IEnumerable<HistoryRow> GetHistory()
        {
            return _historyRepo.GetAll().ToList();
        }

        public IEnumerable<SitemapRow> GetSitemap(string historyRowId)
        {
            return _smRepo.Where(e => e.HistoryRowId == historyRowId);
        }

        public void Dispose()
        {
            _historyRepo.Dispose();
            _smRepo.Dispose();
        }
    }
}
