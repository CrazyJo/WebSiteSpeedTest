using Core.Model;
using Microsoft.AspNet.SignalR;
using WebSiteSpeedTest.Models;

namespace WebSiteSpeedTest.Infrastructure
{
    public class SignalrWorker<THub> where THub : Hub
    {
        readonly IHubContext _hubContextcontext = GlobalHost.ConnectionManager.GetHubContext<THub>();

        public void DisplayMessage(MeasurementResult message)
        {
            //_hubContextcontext.Clients.All.displayMessage(message);
            //todo: постит всем юзерам
            _hubContextcontext.Clients.All.displayMessage(new MeasurementResultViewModel(message.Url, $"{message.MinTime.TotalSeconds:N2}", $"{message.MaxTime.TotalSeconds:N2}"));
        }
    }
}