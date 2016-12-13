namespace UtilitiesPackage.WebtesterPack
{
    public class PageTestedArg : TestArg
    {
        public PageTestedResult PageTestedResult { get; set; }

        public PageTestedArg(PageTestedResult pageTestedResult, WebTestContext context) : base(context)
        {
            PageTestedResult = pageTestedResult;
        }
    }
}
