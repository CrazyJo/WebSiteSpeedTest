﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Repository
{
    public interface IRepo<T> : IDisposable
    {
        void Delete(T obj);

        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        T Get(int id);

        IQueryable<T> GetAll();

        void AddRange(IEnumerable<T> entities);

        void Insert(T obj);

        void Save();

        void Update(T obj);

        void Update(int id, T obj);

        IList<T> Where(Expression<Func<T, bool>> predicate);
    }
}
