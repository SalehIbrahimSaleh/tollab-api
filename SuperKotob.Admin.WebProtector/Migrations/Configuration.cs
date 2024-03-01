namespace SuperMatjar.WebProtector.Migrations
{
    using Core;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<SuperMatjar.WebProtector.ProtectorDbContext>
    {
        public Configuration()
        {
            //mozaly: remove these values on production
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SuperMatjar.WebProtector.ProtectorDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            try
            {
                context.Clients.AddOrUpdate(
                  p => p.Id,
                  new ProtectedClient
                  {
                      Id = "mobile_app",
                      IsActive = true,
                      IsConfidential = false,
                      RefreshTokenLifetime = 20
                  }
                );
            }
            catch (Exception ex)
            {
                var x = ex.GetBaseException();
            }

        }
    }
}
