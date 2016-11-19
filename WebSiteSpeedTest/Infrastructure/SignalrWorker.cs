using Core;
using Core.Model;
using Microsoft.AspNet.SignalR;
using WebSiteSpeedTest.Infrastructure.Extensions;

namespace WebSiteSpeedTest.Infrastructure
{
    public class SignalrWorker<THub> : IMeasurementResultDisplayer where THub : Hub
    {
        private readonly IHubContext _hubContextcontext = GlobalHost.ConnectionManager.GetHubContext<THub>();
        private readonly string _connectionId;

        public SignalrWorker(string connectionId)
        {
            _connectionId = connectionId;
        }

        public void Display(MeasurementResult message)
        {
            _hubContextcontext.Clients.Client(_connectionId).displayMessage(message.ToViewModel());
        }
    }
}