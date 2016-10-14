using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UtilitiesPackage
{
    public static class SitemapWorker
    {
        public static async Task<IEnumerable<string>> ParseSitemapFile(string url)
        {
            XmlDocument rssXmlDoc = new XmlDocument();

            // Load the Sitemap file from the Sitemap URL
            try
            {
                rssXmlDoc.Load(url);

                IEnumerable<string> listResult;
                XmlNamespaceManager nsmgr;
                // Iterate through the top level nodes and find the "urlset" node. 
                foreach (XmlNode topNode in rssXmlDoc.ChildNodes)
                {
                    var nodeName = topNode.Name.ToLower();
                    if (nodeName == "urlset" || nodeName == "sitemapindex")
                    {
                        // Use the Namespace Manager, so that we can fetch nodes using the namespace
                        nsmgr = new XmlNamespaceManager(rssXmlDoc.NameTable);
                        nsmgr.AddNamespace("ns", topNode.NamespaceURI);

                        // Get all URL nodes and iterate through it.
                        XmlNodeList urlNodes = topNode.ChildNodes;

                        listResult = await GetUrls(urlNodes, nsmgr);

                        if (listResult.Count() > 0 && nodeName == "sitemapindex")
                        {
                            var tempList = new ConcurrentQueue<string>();

                            await listResult.ForEachAsync(100, async e =>
                            {
                                foreach (var t in await ParseSitemapFile(e))
                                {
                                    tempList.Enqueue(t);
                                }
                            });

                            //foreach (var u in listResult)
                            //{
                            //    tempList.AddRange(await ParseSitemapFile(u));
                            //}

                            if (tempList.Count > 0)
                                return tempList;
                        }

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



            await listNodes.ForEachAsync<XmlNode>(urlNode =>
            {
                XmlNode locNode = urlNode.SelectSingleNode("ns:loc", nsmgr);
                if (locNode != null)
                {
                    resultList.Enqueue(locNode.InnerText);
                }
            });

            #region ActionBlock

            //var xmlBlock = new ActionBlock<XmlNode>(urlNode =>
            //{
            //    XmlNode locNode = urlNode.SelectSingleNode("ns:loc", nsmgr);

            //    if (locNode != null)
            //    {
            //        resultList.Add(locNode.InnerText);
            //    }

            //}, new ExecutionDataflowBlockOptions{ MaxDegreeOfParallelism = Environment.ProcessorCount });


            //foreach (XmlNode node in listNodes)
            //{
            //    await xmlBlock.SendAsync(node);
            //}

            //await xmlBlock.Completion;

            #endregion

            #region Synchronously

            //foreach (XmlNode urlNode in listNodes)
            //{
            //    // Get the "loc" node and retrieve the inner text.
            //    XmlNode locNode = urlNode.SelectSingleNode("ns:loc", nsmgr);

            //    if (locNode != null)
            //    {
            //        resultList.Add(locNode.InnerText);
            //    }
            //}
            #endregion

            return resultList;
        }
    }
}
