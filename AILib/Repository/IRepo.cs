using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Repository
{
    public interface IRepo<T> : IDisposable
    {
        void Delete(T obj);

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        T Get(int id);

        IQueryable<T> GetAll();

        void Add(T obj);

        void AddRange(IEnumerable<T> entities);

        void Insert(T obj);

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of objects written to the underlying database.</returns>
        Task<int> SaveAsync();

        void Save();

        void Update(T obj);

        void Update(int id, T obj);

        IList<T> Where(Expression<Func<T, bool>> predicate);
    }
}
