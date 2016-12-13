using System;

namespace UtilitiesPackage.WebtesterPack
{
    public class TestOptions
    {
        public TestOptions()
        {
            PageReloadNumber = 2;
            MaxDegreeOfParallelism = Environment.ProcessorCount;
        }

        /// <summary>
        /// The number of times a page reload
        /// </summary>
        public int PageReloadNumber { get; set; }

        public int MaxDegreeOfParallelism { get; set; }
    }
}
