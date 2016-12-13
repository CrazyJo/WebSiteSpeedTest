using System;
using System.Threading;
using System.Threading.Tasks;
using Core;
using Core.Model;
using UtilitiesPackage.WebtesterPack;

namespace UtilitiesPackage
{
    /// <summary>
    /// Web Performance Test
    /// </summary>
    public class WPTest
    {
        public IDisplayer Displayer { get; set; }
        public IStorage Storage { get; set; }
        protected IWebTester Tester;
        protected IFormater<PageTestedResult, MeasurementResult> Formater;
        protected WebTestResult TestRes;
        protected CancellationTokenSource TokenSource;
        protected object locker = new object();

        public WPTest(IDisplayer displayer, IStorage storage) : this(displayer, storage, null)
        {
        }

        public WPTest(IDisplayer displayer, IStorage storage, CancellationTokenSource tokenSource)
        {
            TokenSource = tokenSource ?? new CancellationTokenSource();
            Displayer = displayer;
            Storage = storage;
            Tester = new WebTester(TokenSource, null);
            Tester.PageTestedAsync += Tester_PageTestedAsync;
            Formater = new TestResultFormater();
        }

        private void Tester_PageTestedAsync(object sender, PageTestedArg e)
        {
            try
            {
                var temp = Formater.Format(e.PageTestedResult);
                Displayer?.Display(temp);
                lock (locker)
                    Storage?.Add(temp);
            }
            catch (Exception exception)
            {
                TestRes.Exception = exception;
                TokenSource.Cancel();
            }
        }

        public async Task<WebTestResult> Test(string url)
        {
            Uri uri;
            TestRes = new WebTestResult();

            if (!UrlVerify(url, out uri))
            {
                TestRes.Exception = new Exception("Wrong Url");
            }
            else
            {
                try
                {
                    TestRes = await Tester.BeginTest(uri).ConfigureAwait(false);
                    if (Storage != null && TestRes.TestCompletedSuccessfully)
                        await Storage.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    TestRes.Exception = e;
                    return TestRes;
                }
                Formater.Dispose();
            }

            return TestRes;
        }

        protected virtual bool UrlVerify(string url, out Uri uri)
        {
            return Verificator.UrlVerify(url, out uri);
        }
    }
}
