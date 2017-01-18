using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.SignalR;

namespace SignalTest.Hubs
{
    public class RefreshDataTimer : IRegisteredObject
    {
        private readonly IHubContext _uptimeHub;
        private Timer _timer;

        public RefreshDataTimer()
        {
            _uptimeHub = GlobalHost.ConnectionManager.GetHubContext<RefreshDataHub>();
            HostingEnvironment.RegisterObject(this);

            StartTimer();
        }

        private void StartTimer()
        {
            _timer = new Timer(BroadcastUptimeToClients, null, 0, 30000);
        }

        private void BroadcastUptimeToClients(object state)
        {
            var data = new DataStore().RefreshData();
            _uptimeHub.Clients.All.refreshData(data);
        }

        public void Stop(bool immediate)
        {
            _timer.Dispose();

            HostingEnvironment.UnregisterObject(this);
        }
    }
}