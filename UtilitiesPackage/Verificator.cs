using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesPackage
{
    public static class Verificator
    {
        public static bool UrlVerify(string url, out Uri uri)
        {
            try
            {
                uri = new Uri(url);
            }
            catch (Exception)
            {
                uri = null;
                return false;
            }
            return true;
        }
    }
}
