using System;
using System.Threading.Tasks;

namespace UtilitiesPackage.WebtesterPack
{
    public interface IWebTester
    {
        event EventHandler<PageTestRefusedArg> PageTestRefusedAsync;
        event EventHandler<PageTestedArg> PageTestedAsync;
        Task<WebTestResult> BeginTest(Uri uri);
    }
}
