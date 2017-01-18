using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.SignalR;
using SignalTest.Context;
using SignalTest.Migrations;

namespace SignalTest.Hubs
{
    public class RefreshThroughSqlDependency : IRegisteredObject
    {
        private RefreshDataContext context;
        private IHubContext _uptimeHub;

        public RefreshThroughSqlDependency()
        {
            HostingEnvironment.RegisterObject(this);
            _uptimeHub = GlobalHost.ConnectionManager.GetHubContext<RefreshDataHub>();
            context = new RefreshDataContext();
            context.Database.Initialize(true);
            SqlDependency.Start(context.Database.Connection.ConnectionString);
            InitSqlDependency();
        }

        private void InitSqlDependency()
        {
            var command = context.Database.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT Id, Information FROM dbo.DummyInformations";
            var dep = new SqlDependency(command as SqlCommand);
            dep.OnChange += Changed;
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
            var dr = command.ExecuteReader();
            dr.Dispose();
        }

        private void Changed(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                var data = new DataStore().RefreshData();
                _uptimeHub.Clients.All.refreshData(data);
            }

            var dependency = sender as SqlDependency;
            if (dependency != null)
            {
                dependency.OnChange -= Changed;
            }
            InitSqlDependency();
        }

        public void Stop(bool immediate)
        {
            SqlDependency.Stop(context.Database.Connection.ConnectionString);
            context.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }
    }
}