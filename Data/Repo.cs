using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using Core.Repository;


namespace Data
{
    public class Repo<T> : IRepo<T> where T : class , new()
    {

        protected readonly DbContext DbContext;

        public Repo(IDbContextFactory dbContextFactory)
        {
            DbContext = dbContextFactory.GetContext();
        }

        public virtual void Delete(T obj)
        {
            DbContext.Set<T>().Remove(obj);
            DbContext.SaveChanges();
        }

        public T Get(int id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

        public void AddRange(IEnumerable<T> entities)
        {
            DbContext.Set<T>().AddRange(entities);
            DbContext.SaveChanges();
        }

        public void Insert(T obj)
        {
            DbContext.Set<T>().Add(obj);
            DbContext.SaveChanges();
        }

        public void Save()
        {
            DbContext.SaveChanges();
        }

        public void Update(T obj)
        {
            DbContext.Set<T>().AddOrUpdate(obj);
            DbContext.SaveChanges();
        }

        public void Update(int id, T obj)
        {
            DbContext.Set<T>().AddOrUpdate(obj);
            DbContext.SaveChanges();
        }

        public virtual IList<T> Where(Expression<Func<T, bool>> predicate)
        {
            return DbContext.Set<T>().Where(predicate).ToList();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = GetAll();
            foreach (var includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }

            return queryable;
        }
    }

}