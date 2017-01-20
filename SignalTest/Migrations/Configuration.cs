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
            var totalInDb = context.DummyInformations.Count();
            if (totalInDb < 10000000)
            {
                var ctx = new RefreshDataContext();
                ctx.Configuration.AutoDetectChangesEnabled = false;
                ctx.Configuration.ValidateOnSaveEnabled = false;
                for (int i = 0; i < 10000000 - totalInDb; i++)
                {
                    ctx.DummyInformations.Add(new DummyInformation { Information = i });
                    if (i % 100000 == 0)
                    {
                        ctx.SaveChanges();
                        ctx.Dispose();
                        ctx = new RefreshDataContext();
                        ctx.Configuration.AutoDetectChangesEnabled = false;
                        ctx.Configuration.ValidateOnSaveEnabled = false;
                    }
                }

                ctx.SaveChanges();
            }
        }
    }
}
