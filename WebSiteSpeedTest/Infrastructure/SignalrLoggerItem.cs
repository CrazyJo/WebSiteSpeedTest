using AILib;
using Microsoft.AspNet.SignalR;

namespace WebSiteSpeedTest.Infrastructure
{
    public class SignalrLoggerItem<T, THub> : ILoggerItem<T> where THub : Hub
    {
        readonly IHubContext _hubContextcontext = GlobalHost.ConnectionManager.GetHubContext<THub>();

        public void Log(T message)
        {
            //var _hubContextcontext = GlobalHost.ConnectionManager.GetHubContext<THub>();
            _hubContextcontext.Clients.All.displayMessage(message);
        }
    }
}