using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Extensions.Regex
{
    public static class RegexExtensions
    {
        public static string GetExtension(this string value)
        {
            var r = new System.Text.RegularExpressions.Regex(@"\.([0-9a-z]+)(?=[?#])|(\.)(?:[\w]+)$");
            return r.Match(value).Value;

        }

        public static bool BeginWith(this string source, string value)
        {
            var r = new System.Text.RegularExpressions.Regex($"^{value}");
            return r.IsMatch(source);
        }

        public static IEnumerable<string> GetSitemapUrls(this string content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var result = new List<string>();

            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"(?<=^sitemap.*)(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?",
                RegexOptions.Multiline | RegexOptions.IgnoreCase);

            foreach (Match match in r.Matches(content))
            {
                result.Add(match.Value);
            }

            return result;
        }

        public static string GetDomain(this string url)
        {
            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(@"^(?:https?:\/\/)?(?:[^@\/\n]+@)?(?:www\.)?([^:\/\n]+)");
            return r.Match(url).Value;
        }

        public static bool TryGetDomain(this string url, out string result)
        {
            result = GetDomain(url);
            return result != null;
        }
    }
}
