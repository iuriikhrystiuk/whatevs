using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.AspNet.SignalR;
using SignalTest.Context;

namespace SignalTest.Hubs
{
    public class RefreshThroughSqlDependency : IRegisteredObject
    {
        private RefreshDataContext context;
        private IHubContext _uptimeHub;
        private const int timeout = 1000;

        public RefreshThroughSqlDependency()
        {
            HostingEnvironment.RegisterObject(this);
            _uptimeHub = GlobalHost.ConnectionManager.GetHubContext<RefreshDataHub>();
            context = new RefreshDataContext();
            context.Database.Connection.StateChange += ConnectionChanged;
            context.Database.Initialize(true);
            InitSqlDependency();
        }

        private void InitSqlDependency()
        {
            var command = context.Database.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT Id, Information FROM dbo.DummyInformations WHERE Id > 2";
            try
            {
                var dep = new SqlDependency(command as SqlCommand);
                dep.OnChange += Changed;
                if (command.Connection.State != ConnectionState.Open)
                {
                    SqlDependency.Start(context.Database.Connection.ConnectionString);
                    command.Connection.Open();
                }

                // executenonquery is slower then executereader
                // but disposing of reader takes the same amount of time as waiting for result of non query execution.
                // we still might want to bump connection timeout a little
                command.ExecuteNonQuery();

                command.Dispose();
            }
            catch (Exception)
            {
                Task.Delay(timeout).ContinueWith(t => InitSqlDependency());
            }
        }

        private void ConnectionChanged(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Closed || e.CurrentState == ConnectionState.Broken)
            {
                SqlDependency.Stop(context.Database.Connection.ConnectionString);

                Task.Delay(timeout).ContinueWith(t =>
                {
                    InitSqlDependency();
                });
            }
        }

        private void Changed(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                var dependency = sender as SqlDependency;
                if (dependency != null)
                {
                    dependency.OnChange -= Changed;
                }

                Task.Delay(timeout).ContinueWith(t => InitSqlDependency());

                var data = new DataStore().RefreshData();
                _uptimeHub.Clients.All.refreshData(data);
            }
        }

        public void Stop(bool immediate)
        {
            SqlDependency.Stop(context.Database.Connection.ConnectionString);
            context.Dispose();
            HostingEnvironment.UnregisterObject(this);
        }
    }
}