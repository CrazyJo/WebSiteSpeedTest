using Core.Model;
using Microsoft.AspNet.SignalR;
using WebSiteSpeedTest.Infrastructure.Extensions;

namespace WebSiteSpeedTest.Infrastructure
{
    public class SignalrWorker<THub> where THub : Hub
    {
        readonly IHubContext _hubContextcontext = GlobalHost.ConnectionManager.GetHubContext<THub>();

        public void DisplayMessage(MeasurementResult message)
        {
            // todo: исправить что б отправлял сообщегия только текущему юзеру
            _hubContextcontext.Clients.All.displayMessage(message.ToViewModel());
        }
    }
}