using System.Threading;

namespace UtilitiesPackage.WebtesterPack
{
    public class WebTestContext
    {
        public int TestedPagesCount;
        public CancellationTokenSource CancellationTokenSource { get; set; }
        public TestOptions TestOptions { get; set; }
    }
}
