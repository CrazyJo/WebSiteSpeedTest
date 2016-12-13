using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Core.Collection;
using Extensions.Parallelism;
using Extensions.Regex;

namespace UtilitiesPackage
{
    public static class SitemapWorker
    {
        static readonly HttpClient HttpClient = new HttpClient();

        public static event Action<string> FoundUrl;

        static void OnFoundUrl(string url)
        {
            Volatile.Read(ref FoundUrl)?.Invoke(url);
        }

        /// <summary>
        /// Get sitemap links from robots.txt
        /// </summary>
        public static async Task<IEnumerable<string>> GetFromRobots(string domain)
        {
            var response = await LoadPage(domain + "/robots.txt").ConfigureAwait(false);
            if (response != null)
            {
                var content = await response.ReadAsStringAsync().ConfigureAwait(false);
                return content.GetSitemapUrls();
            }
            return new List<string>();
        }

        public static async Task<XmlDocument> FindeFirstSitemapDoc(string url)
        {
            XmlDocument results;
            var domain = url.GetDomain();
            var sitemapXml = await LoadDoc(domain + "/sitemap.xml").ConfigureAwait(false);
            if (sitemapXml != null)
            {
                results = sitemapXml;
            }
            else
            {
                var sitemapUrls = await GetFromRobots(domain);
                var urls = sitemapUrls as IList<string> ?? sitemapUrls.ToList();
                if (!urls.Any()) return null;
                var path = urls.First();
                results = await LoadDoc(path).ConfigureAwait(false);
            }

            return results;
        }

        public static async Task<IEnumerable<XmlDocument>> FindSitemap(string url, int numberOfDoc = int.MaxValue, string specificUrl = null)
        {
            var results = new ConcurrentQueue<XmlDocument>();
            var domain = url.GetDomain();

            var sitemapXml = await LoadDoc(domain + "/sitemap.xml").ConfigureAwait(false);
            if (sitemapXml != null)
            {
                results.Enqueue(sitemapXml);
            }
            else
            {
                var sitemapLinks = await GetFromRobots(domain);
                if (specificUrl == null)
                {
                    var temp = sitemapLinks.Take(numberOfDoc);
                    await temp.ForEach(async uri =>
                    {
                        var doc = await LoadDoc(uri);
                        if (doc != null)
                            results.Enqueue(doc);
                    }).ConfigureAwait(false);
                }
                else
                {
                    var temp = sitemapLinks.First(link => link.Contains(specificUrl));
                    if (temp != null)
                    {
                        var doc = await LoadDoc(temp);
                        if (doc != null)
                            results.Enqueue(doc);
                    }
                    else
                    {
                        throw new KeyNotFoundException("Current sitemap url doesn't contain this specific url.");
                    }
                }
            }

            return results;
        }

        static async Task<XmlDocument> LoadDoc(string uri)
        {
            var response = await LoadPage(uri);
            if (response == null) return null;
            var responseStream = await response.ReadAsStreamAsync().ConfigureAwait(false);

            var tempDoc = new XmlDocument();
            var extension = uri.GetExtension();

            try
            {
                switch (extension)
                {
                    case ".xml":
                        {
                            tempDoc.Load(responseStream);
                            break;
                        }

                    case ".gz":
                        {
                            using (var zip = new GZipStream(responseStream, CompressionMode.Decompress))
                                tempDoc.Load(zip);
                            break;
                        }
                    default:
                        return null;
                        //throw new InvalidOperationException("Сan not load a document with this extension");
                }
            }
            catch (Exception)
            {
                return null;
            }

            return tempDoc;
        }

        static async Task<HttpContent> LoadPage(string uri)
        {
            var response = await HttpClient.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return response.Content;
            return null;
        }

        public static async Task<IEnumerable<string>> ParseSitemapFile(XmlDocument rssXmlDoc)
        {
            if (rssXmlDoc == null)
                throw new ArgumentNullException(nameof(rssXmlDoc));
            var root = rssXmlDoc.DocumentElement;
            if (root == null) throw new InvalidOperationException("This is an empty xml file");
            var localName = root.LocalName;
            if (localName.Equals("urlset", StringComparison.InvariantCultureIgnoreCase))
                return await ParseUrlset(rssXmlDoc, GetNamespace(rssXmlDoc)).ConfigureAwait(false);
            if (!root.LocalName.Equals("sitemapindex", StringComparison.InvariantCultureIgnoreCase))
                throw new InvalidOperationException(nameof(rssXmlDoc) + " it is not sitemap.xml");

            // Use the Namespace Manager, so that we can fetch nodes using the namespace
            var nsmgr = new XmlNamespaceManager(rssXmlDoc.NameTable);
            nsmgr.AddNamespace("ns", root.NamespaceURI);

            // Get all sitemap.xml nodes
            var sitemapNodes = root.ChildNodes;
            var sitemapUrls = await GetUrls(sitemapNodes, nsmgr).ConfigureAwait(false);
            var resultList = new ConcurrentBag<string>();

            //todo
            sitemapUrls = sitemapUrls.ToList().Take(1);

            await sitemapUrls.ForEach(async url =>
            {
                var doc = await LoadDoc(url).ConfigureAwait(false);
                var tempRes = await ParseUrlset(doc, nsmgr).ConfigureAwait(false);
                resultList.AddRange(tempRes);
            }).ConfigureAwait(false);

            return resultList;
        }

        private static XmlNamespaceManager GetNamespace(XmlDocument xmlDoc)
        {
            var nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsmgr.AddNamespace("ns", xmlDoc.DocumentElement.NamespaceURI);
            return nsmgr;
        }

        private static async Task<IEnumerable<string>> ParseUrlset(XmlDocument rssXmlDoc, XmlNamespaceManager nsmgr)
        {
            if (rssXmlDoc == null)
                throw new ArgumentNullException(nameof(rssXmlDoc));

            var root = rssXmlDoc.DocumentElement;
            if (root != null)
                return await GetUrls(root.ChildNodes, nsmgr, true);

            return null;
        }

        private static async Task<IEnumerable<string>> GetUrls(XmlNodeList listNodes, XmlNamespaceManager nsmgr, bool enableEvent = false)
        {
            if (listNodes == null)
                throw new ArgumentNullException(nameof(listNodes));

            var resultList = new ConcurrentQueue<string>();

            await listNodes.ForEach<XmlNode>(node =>
            {
                var locNode = node.SelectSingleNode("ns:loc", nsmgr);

                if (locNode == null) return;
                var url = locNode.InnerText;

                if (enableEvent)
                    OnFoundUrl(url);

                resultList.Enqueue(url);

            }).ConfigureAwait(false);

            return resultList;
        }
    }
}
