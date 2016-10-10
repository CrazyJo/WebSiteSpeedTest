using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AILib
{
    public interface ILoadTimeManager
    {
        /// <summary>
        /// It measures the load time of the site and all its references in sitemap.xml
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<IDictionary<string, TimeSpan>> LoadTimeMeasuringWithSitemapAsync(string url);
    }

}
