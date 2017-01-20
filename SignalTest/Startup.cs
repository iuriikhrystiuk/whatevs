using System.Data.Entity;
using System.Web.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using SignalTest.Context;
using SignalTest.Hubs;
using SignalTest.Migrations;

[assembly: OwinStartup(typeof(SignalTest.Startup))]
namespace SignalTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var populateDb = bool.Parse(WebConfigurationManager.AppSettings["populatedb"]);
            if (populateDb)
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<RefreshDataContext, Configuration>());
            }

            // Any connection or hub wire up and configuration should go here
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true
            };
            app.MapSignalR("/signalr", hubConfiguration);
            new RefreshDataTimer();
            new RefreshThroughSqlDependency();
        }
    }
}