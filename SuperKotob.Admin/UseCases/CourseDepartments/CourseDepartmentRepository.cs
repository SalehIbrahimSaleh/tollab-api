using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.CourseDepartments
{
   public class CourseDepartmentRepository : BaseRepository<CourseDepartment>
    {
        protected override IQueryable<CourseDepartment> SetInculdes(DbSet<CourseDepartment> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Department).Include(it=>it.Course);
        }
        protected override IQueryable<CourseDepartment> SetWhere(IQueryable<CourseDepartment> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var departmentId = requestInputs.GetValue("DepartmentId", default(long?));
            if (departmentId.HasValue && departmentId.Value > 0)
                query = query.Where(item => item.DepartmentId == departmentId.Value);

            var courseId = requestInputs.GetValue("CourseId", default(long?));
            if (courseId.HasValue && courseId.Value > 0)
                query = query.Where(item => item.CourseId == courseId.Value);

            return query;
        }



    }
}
