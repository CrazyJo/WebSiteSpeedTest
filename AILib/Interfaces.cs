using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AILib
{
    public interface ILoggerItem<in T>
    {
        void Log(T message);
    }

    public interface ILoadTimeManager : IDisposable
    {
        /// <summary>
        /// It measures the load time of the site and all its references in sitemap.xml
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> LoadTimeMeasuringWithSitemapAsync<TResult>(string url);
    }

}
