using Microsoft.AspNet.SignalR;

namespace WebSiteSpeedTest.Infrastructure
{
    public class SignalrWorker<THub> : ISender where THub : Hub
    {
        private readonly IHubContext _hubContextcontext = GlobalHost.ConnectionManager.GetHubContext<THub>();
        private readonly string _connectionId;

        public SignalrWorker(string connectionId)
        {
            _connectionId = connectionId;
        }

        public void Send<T>(T message)
        {
            Send(_connectionId, message);
        }

        public void Send<T>(string connectionId, T message)
        {
            _hubContextcontext.Clients.Client(connectionId).displayMessage(message);
        }
    }
}