using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models.Views;

namespace Tollab.Admin.UseCases.TeachersSubjects
{
   public class TeachersSubjectRepository : BaseRepository<TeachersSubject>
    {
        protected override IQueryable<TeachersSubject> SetInculdes(DbSet<TeachersSubject> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Department).Include(i => i.Teacher);
        }
        protected override IQueryable<TeachersSubject> SetWhere(IQueryable<TeachersSubject> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var departmentId = requestInputs.GetValue("DepartmentId", default(long?));
            if (departmentId.HasValue && departmentId.Value > 0)
                query = query.Where(item => item.DepartmentId == departmentId.Value);

            var TeacherId = requestInputs.GetValue("TeacherId", default(long?));
            if (TeacherId.HasValue && TeacherId.Value > 0)
                query = query.Where(item => item.TeacherId == TeacherId.Value);
            return query;
        }



    }
}
