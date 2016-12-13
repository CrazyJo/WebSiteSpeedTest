using System;
using Core.Model;
using UtilitiesPackage;
using UtilitiesPackage.WebtesterPack;
using WebSiteSpeedTest.Hubs;
using WebSiteSpeedTest.Infrastructure.Extensions;

namespace WebSiteSpeedTest.Infrastructure
{
    public class ClientSideSender : IDisplayer
    {
        protected ISender _sender;
        protected string _connectionId;

        public ClientSideSender(string connectionId) : this(connectionId, null)
        {
        }

        public ClientSideSender(string connectionId, ISender sender)
        {
            _connectionId = connectionId;
            _sender = sender ?? new SignalrWorker<NotificationHub>(connectionId);
        }

        public void Display(MeasurementResult value)
        {
            _sender.Send(value.ToViewModel());
        }
    }
}