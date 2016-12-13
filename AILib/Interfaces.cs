using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;

namespace Core
{
    public interface IMeasurementResultDisplayer
    {
        void Display(MeasurementResult message);
    }

    public interface IStorage
    {
        void Add<T>(T obj);

        /// <summary>
        /// Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>A task that represents the asynchronous save operation. The task result contains the number of objects written to the underlying database.</returns>
        Task<int> SaveChangesAsync();
    }
}
