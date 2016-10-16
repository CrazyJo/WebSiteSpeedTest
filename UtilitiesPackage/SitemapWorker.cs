using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace UtilitiesPackage
{
    public static class SitemapWorker
    {
        static readonly HttpClient HttpClient = new HttpClient();
        public static async Task<IEnumerable<XmlDocument>> FindSitemap(string url)
        {
            var results = new ConcurrentQueue<XmlDocument>();
            var domain = url.GetDomain();

            try
            {
                results.Enqueue(await LoadDoc(domain + "/sitemap.xml").ConfigureAwait(false));
            }
            catch (Exception e)
            {
                // doesn't contain a "sitemap.xml"
                try
                {
                    var content = await HttpClient.GetStringAsync(domain + "/robots.txt").ConfigureAwait(false);


                    await content.GetUrlsFromRobotsTxt().ForEach(async uri =>
                    {
                        var doc = await LoadDoc(uri);
                        if (doc != null)
                            results.Enqueue(doc);
                    }).ConfigureAwait(false);

                    #region Synchronously

                    //foreach (var uri in content.GetUrlsFromRobotsTxt())
                    //{
                    //    try
                    //    {
                    //        results.Add(await LoadDoc(uri));
                    //    }
                    //    catch (Exception exception)
                    //    {
                    //        ;
                    //    }
                    //}

                    #endregion

                }
                catch (Exception exception)
                {
                    // doesn't contain a "robots.txt"
                    ;
                }
            }

            return results;
        }

        static async Task<XmlDocument> LoadDoc(string uri)
        {
            XmlDocument tempDoc = null;
            //var client = new HttpClient();

            try
            {
                tempDoc = new XmlDocument();
                var t = await HttpClient.GetStringAsync(uri).ConfigureAwait(false);
                //var t = await client.GetStringAsync(uri);
                tempDoc.LoadXml(t);
            }
            catch (Exception exception)
            {
                throw;
            }

            return tempDoc;
        }

        public static async Task<IEnumerable<string>> ParseSitemapFile(XmlDocument rssXmlDoc)
        {
            if (rssXmlDoc == null)
                throw new ArgumentNullException(nameof(rssXmlDoc));

            try
            {
                IEnumerable<string> listResult;
                XmlNamespaceManager nsmgr;

                // Iterate through the top level nodes and find the "urlset" node. 
                foreach (XmlNode topNode in rssXmlDoc.ChildNodes)
                {
                    var nodeName = topNode.Name.ToLower();
                    if (nodeName.Contains("urlset") || nodeName == "sitemapindex")
                    {
                        // Use the Namespace Manager, so that we can fetch nodes using the namespace
                        nsmgr = new XmlNamespaceManager(rssXmlDoc.NameTable);
                        nsmgr.AddNamespace("ns", topNode.NamespaceURI);

                        // Get all URL nodes and iterate through it.
                        XmlNodeList urlNodes = topNode.ChildNodes;

                        listResult = await GetUrls(urlNodes, nsmgr).ConfigureAwait(false);


                        #region The first level of nesting "sitemapIndex"

                        if (listResult.Count() > 0 && nodeName == "sitemapindex")
                        {
                            var tempList = new ConcurrentQueue<string>();

                            // It is necessary to parallelize the computation of all * .xml links 
                            await listResult.ForEach(async tUrl =>
                            {
                                foreach (var t in await ParseSitemapFile(await LoadDoc(tUrl)))
                                {
                                    tempList.Enqueue(t);
                                }
                            }).ConfigureAwait(false);

                            if (tempList.Count > 0)
                                return tempList;

                            return new List<string>();
                        }

                        #endregion

                        return listResult;
                    }
                }
            }
            catch (Exception e)
            {
                ;
                //return new List<string>();
            }

            return new List<string>();
        }

        public static async Task<IEnumerable<string>> GetUrls(XmlNodeList listNodes, XmlNamespaceManager nsmgr)
        {
            if (listNodes == null)
                throw new ArgumentNullException(nameof(listNodes));


            var resultList = new ConcurrentQueue<string>();

            await listNodes.ForEach<XmlNode>(node =>
            {
                XmlNode locNode = node.SelectSingleNode("ns:loc", nsmgr);

                if (locNode != null)
                    resultList.Enqueue(locNode.InnerText);
            }).ConfigureAwait(false);

            return resultList;
        }
    }

}
