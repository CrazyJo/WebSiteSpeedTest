using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSiteSpeedTest.Infrastructure
{
    public interface ISender
    {
        void Send<T>(T message);
    }
}