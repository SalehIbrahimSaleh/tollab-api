using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Categories
{
   public class OfflinePackageRepository : BaseRepository<OfflinePackage>
    {
        protected override IQueryable<OfflinePackage> SetInculdes(DbSet<OfflinePackage> dbSet, RequestInputs requestInputs)
        {
            return dbSet.AsQueryable();
        }
        protected override IQueryable<OfflinePackage> SetWhere(IQueryable<OfflinePackage> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            return query;
        }
    }
}
