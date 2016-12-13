using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Collection;
using Core.Model;
using Core;

namespace Data
{
    public class Committer : IDisposable, IStorage
    {
        private readonly Repo<HistoryRow> _historyRepo;
        private readonly Repo<SitemapRow> _smRepo;

        public Committer()
        {
            IDbContextFactory db = new DbContextFactory();
            _historyRepo = new Repo<HistoryRow>(db);
            _smRepo = new Repo<SitemapRow>(db);
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
            return GetHistory().OrderByDescending(i => i.Date).TakeRange(startIndex, count).ToList();
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

        public void Add(HistoryRow row)
        {
            _historyRepo.Add(row);
        }
        public void Add(SitemapRow row)
        {
            _smRepo.Add(row);
        }

        public void Add<T>(T obj)
        {
            SitemapRow sr = obj as SitemapRow;
            if (sr != null)
            {
                Add(sr);
            }
            else
            {
                HistoryRow hr = obj as HistoryRow;
                if (hr != null)
                {
                    Add(hr);
                }
                else
                {
                    throw new ArgumentException("Parameter type must be either HistoryRow or SitemapRow.", nameof(obj));
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _historyRepo.SaveAsync().ConfigureAwait(false) + await _smRepo.SaveAsync().ConfigureAwait(false);
        }
    }
}
