using Microsoft.AspNet.Identity.EntityFramework;
using SuperMatjar.WebProtector.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector
{
    public class ProtectorDbContext : IdentityDbContext<ProtectedUser>
    {
        public ProtectorDbContext()
            : base("AuthDbConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<ProtectedClient> Clients { get; set; }
        public DbSet<ProtectedRefreshToken> RefreshTokens { get; set; }

        public static ProtectorDbContext Create()
        {
            return new ProtectorDbContext();
        }

    }
}
