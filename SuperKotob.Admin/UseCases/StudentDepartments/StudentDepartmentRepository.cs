using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentDepartments
{
   public class StudentDepartmentRepository : BaseRepository<StudentDepartment>
    {
        protected override IQueryable<StudentDepartment> SetInculdes(DbSet<StudentDepartment> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Student).Include(item => item.Department);
        }
        protected override IQueryable<StudentDepartment> SetWhere(IQueryable<StudentDepartment> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var departmentId = requestInputs.GetValue("DepartmentId", default(long?));
            if (departmentId.HasValue && departmentId.Value > 0)
                query = query.Where(item => item.DepartmentId == departmentId.Value);

            var studentId = requestInputs.GetValue("StudentId", default(long?));
            if (studentId.HasValue && studentId.Value > 0)
                query = query.Where(item => item.StudentId == studentId.Value);

            return query;
        }
    }
}
