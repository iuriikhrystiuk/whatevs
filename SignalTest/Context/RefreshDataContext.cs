using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SignalTest.Context
{
    public class RefreshDataContext : DbContext
    {
        public IDbSet<DummyInformation> DummyInformations { get; set; }
    }
}