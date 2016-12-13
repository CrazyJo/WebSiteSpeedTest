using System;

namespace UtilitiesPackage.WebtesterPack
{
    public class TestArg : EventArgs
    {
        public WebTestContext Context { get; set; }

        public TestArg(WebTestContext context)
        {
            Context = context;
        }
    }
}
