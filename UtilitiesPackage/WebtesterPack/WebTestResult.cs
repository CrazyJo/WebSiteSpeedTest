using System;

namespace UtilitiesPackage.WebtesterPack
{
    public class WebTestResult
    {
        public bool TestCompletedSuccessfully => Exception == null;
        public virtual Exception Exception { get; set; }
        public int CrawledPagesCount { get; set; }
        public int TestedPagesCount { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}
