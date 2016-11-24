using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace WebSiteSpeedTest.Infrastructure
{
    public static class WebTool
    {
        public static void SetUserLocale(string culture = null, 
            string uiCulture = null, 
            string currencySymbol = null, 
            bool setUiCulture = true, 
            string allowedLocales = null)
        {
            // Use browser detection in ASP.NET
            if (string.IsNullOrEmpty(culture) && HttpContext.Current != null)
            {
                HttpRequest Request = HttpContext.Current.Request;

                // if no user lang leave existing but make writable
                if (Request.UserLanguages == null)
                {
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
                    if (setUiCulture)
                        Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture.Clone() as CultureInfo;

                    return;
                }

                culture = Request.UserLanguages[0];
            }
            else
                culture = culture.ToLower();

            if (!string.IsNullOrEmpty(uiCulture))
                setUiCulture = true;

            if (!string.IsNullOrEmpty(culture) && !string.IsNullOrEmpty(allowedLocales))
            {
                allowedLocales = "," + allowedLocales.ToLower() + ",";
                if (!allowedLocales.Contains("," + culture + ","))
                {
                    int i = culture.IndexOf('-');
                    if (i > 0)
                    {
                        culture = culture.Substring(0, i);
                        if (!allowedLocales.Contains("," + culture + ","))
                        {
                            // Always create writable CultureInfo
                            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;
                            if (setUiCulture)
                                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture.Clone() as CultureInfo;

                            return;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(culture))
                culture = CultureInfo.InstalledUICulture.IetfLanguageTag;

            if (string.IsNullOrEmpty(uiCulture))
                uiCulture = culture;

            try
            {
                CultureInfo Culture = new CultureInfo(culture);

                if (currencySymbol != null && currencySymbol != "")
                    Culture.NumberFormat.CurrencySymbol = currencySymbol;

                Thread.CurrentThread.CurrentCulture = Culture;

                if (setUiCulture)
                {
                    var UICulture = new CultureInfo(uiCulture);
                    Thread.CurrentThread.CurrentUICulture = UICulture;
                }
            }
            catch { }
        }
    }
}