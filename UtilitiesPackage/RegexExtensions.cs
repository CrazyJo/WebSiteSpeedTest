using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UtilitiesPackage
{
    public static class RegexExtensions
    {
        public static string GetExtension(this string value)
        {
            var r = new Regex(@"\.([0-9a-z]+)(?=[?#])|(\.)(?:[\w]+)$");
            return r.Match(value).Value;

        }

        public static IEnumerable<string> GetSitemapUrls(this string content)
        {
            var result = new List<string>();

            Regex r = new Regex(@"(?<=^sitemap.*)(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in r.Matches(content))
            {
                result.Add(match.Value);
            }

            return result;
        }

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
