using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using SignalTest.Hubs;

[assembly: OwinStartup(typeof(SignalTest.Startup))]
namespace SignalTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true
            };
            app.MapSignalR("/signalr", hubConfiguration);
            new RefreshDataTimer();
        }
    }
}