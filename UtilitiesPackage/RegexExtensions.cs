using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UtilitiesPackage
{
    public static class RegexExtensions
    {
        public static string GetDomain(this string url)
        {
            Regex r = new Regex(@"^(?:https?:\/\/)?(?:[^@\/\n]+@)?(?:www\.)?([^:\/\n]+)");
            return r.Match(url).Value;
        }

        public static bool TryGetDomain(this string url, out string result)
        {
            result = GetDomain(url);
            return result != null;
        }
    }
}
