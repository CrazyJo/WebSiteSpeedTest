using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UtilitiesPackage
{
    public static class SitemapWorker
    {
        public static IEnumerable<string> ParseSitemapFile(string url)
        {
            XmlDocument rssXmlDoc = new XmlDocument();

            // Load the Sitemap file from the Sitemap URL
            try
            {
                rssXmlDoc.Load(url);

                ICollection<string> listResult;
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
                        listResult = GetUrls(urlNodes, nsmgr);

                        if (listResult.Count > 0 && nodeName == "sitemapindex")
                        {
                            List<string> tempList = new List<string>();

                            foreach (var u in listResult)
                            {
                                tempList.AddRange(ParseSitemapFile(u));
                            }

                            if (tempList.Count > 0)
                                return tempList;
                        }

                        return listResult;
                    }
                }
            }
            catch (Exception e)
            {
                //throw new Exception("Wrong URL", e);
                return new List<string>();
            }

            return new List<string>();
        }

        public static ICollection<string> GetUrls(XmlNodeList listNodes, XmlNamespaceManager nsmgr)
        {
            List<string> resultList = new List<string>();

            if (listNodes != null)
            {
                foreach (XmlNode urlNode in listNodes)
                {
                    // Get the "loc" node and retrieve the inner text.
                    XmlNode locNode = urlNode.SelectSingleNode("ns:loc", nsmgr);

                    if (locNode != null)
                    {
                        resultList.Add(locNode.InnerText);
                    }
                }
            }

            return resultList;
        }
    }
}
