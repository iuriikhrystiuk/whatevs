using System;
using Microsoft.AspNet.SignalR;

namespace SignalTest.Hubs
{
    public class RefreshDataHub : Hub
    {
        public void SendRefreshMessage()
        {
            Clients.All.refreshData(new Guid());
        }
    }
}