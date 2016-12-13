using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using CrawlerBot.Shell;

namespace CrawlerBot.Core.Parsers
{
    public class AngleSharpLinkParser : LinkParser
    {
        private readonly HtmlParser _parser;

        public AngleSharpLinkParser()
        {
            _parser = new HtmlParser();
        }

        protected override IEnumerable<string> GetHrefValues(CrawledPage crawledPage)
        {
            var rawLinks = _parser.Parse(crawledPage.RawContent).QuerySelectorAll("a[href]");
            var res = new List<string>();
            foreach (var tagA in rawLinks)
            {
                string link = tagA.GetAttribute("href");
                res.Add(link);
            }
            return res;
        }
    }
}
