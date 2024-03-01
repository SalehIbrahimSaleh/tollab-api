using System;
using System.Collections.Generic;
using Model = Tollab.Admin.Data.Models.DisableReason;
using SuperKotob.Admin.Data.Repositories;
using SuperKotob.Admin.Core;
using System.Data.Entity;
using System.Linq;

namespace Tollab.Admin.UseCases.DisableReasons
{
   public class DisableReasonRepository : BaseRepository<Model>
    {
        protected override IQueryable<Model> SetInculdes(DbSet<Model> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Student);
        }
        protected override IQueryable<Model> SetWhere(IQueryable<Model> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var StudentId = requestInputs.GetValue("StudentId", default(long?));
            if (StudentId.HasValue && StudentId.Value > 0)
                query = query.Where(item => item.StudentId == StudentId.Value);
            return query;
        }
    }
}
