using System;
using System.Web.Http;
using SignalTest.Hubs;

namespace SignalTest.Controllers
{
    public class DataController : ApiController
    {
        public DatatItem Get(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new DataStore().GetDataItem(Guid.Empty);
            }

            var guid = Guid.Parse(id);
            return new DataStore().GetDataItem(guid);
        }
    }
}
