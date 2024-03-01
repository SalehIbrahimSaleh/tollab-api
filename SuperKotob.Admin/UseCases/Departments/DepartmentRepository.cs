using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Departments
{
   public class DepartmentRepository : BaseRepository<Department>
    {
        protected override IQueryable<Department> SetInculdes(DbSet<Department> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.SubCategory);
        }
        protected override IQueryable<Department> SetWhere(IQueryable<Department> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var subCategoryId = requestInputs.GetValue("SubCategoryId", default(long?));
            if (subCategoryId.HasValue && subCategoryId.Value > 0)
                query = query.Where(item => item.SubCategoryId == subCategoryId.Value);

            return query;
        }



    }
}
