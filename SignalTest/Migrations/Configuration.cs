using SignalTest.Context;

namespace SignalTest.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SignalTest.Context.RefreshDataContext>
    {
        public Configuration()
        {
        }

        protected override void Seed(SignalTest.Context.RefreshDataContext context)
        {
        }
    }
}
