using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Subjects
{
   public class SubjectRepository : BaseRepository<Subject>
    {
        protected override IQueryable<Subject> SetInculdes(DbSet<Subject> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Department);
        }
        protected override IQueryable<Subject> SetWhere(IQueryable<Subject> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var departmentId = requestInputs.GetValue("DepartmentId", default(long?));
            if (departmentId.HasValue && departmentId.Value > 0)
                query = query.Where(item => item.DepartmentId == departmentId.Value);

            return query;
        }



    }
}
