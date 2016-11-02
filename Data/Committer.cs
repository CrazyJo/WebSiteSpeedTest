using System;
using System.Collections.Generic;
using System.Linq;
using Core.Model;
using UtilitiesPackage;

namespace Data
{
    public class Committer : IDisposable
    {
        private readonly Repo<HistoryRow> _historyRepo;
        private readonly Repo<SitemapRow> _smRepo;

        public Committer()
        {
            IDbContextFactory db = new DbContextFactory();
            _historyRepo = new Repo<HistoryRow>(db);
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

        public IQueryable<HistoryRow> GetHistory()
        {
            return _historyRepo.GetAll();
        }

        public IQueryable<SitemapRow> GetSitemap(string historyRowId)
        {
            return _smRepo.GetAll().Where(i => i.HistoryRowId == historyRowId);
        }

        public IList<HistoryRow> TakePartOfHistoryRows(int startIndex, int count)
        {
            return GetHistory().OrderBy(i => i.Id).TakeRange(startIndex, count).ToList();
        }

        public IList<SitemapRow> TakePartOfSitemapRows(string historyRowId, int startIndex, int count)
        {
            return GetSitemap(historyRowId).OrderBy(i => i.Id).TakeRange(startIndex, count).ToList();
        }

        public void Dispose()
        {
            _historyRepo.Dispose();
            _smRepo.Dispose();
        }
    }
}
