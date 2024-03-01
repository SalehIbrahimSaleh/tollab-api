
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.TeacherSubjects
{
    class TeacherSubjectsRepository : BaseRepository<TeacherSubject>
    {
        protected override IQueryable<TeacherSubject> SetInculdes(DbSet<TeacherSubject> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Subject).Include(item => item.Teacher);
        }
        protected override IQueryable<TeacherSubject> SetWhere(IQueryable<TeacherSubject> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var subjectId = requestInputs.GetValue("SubjectId", default(long?));
            if (subjectId.HasValue && subjectId.Value > 0)
                query = query.Where(item => item.SubjectId == subjectId.Value);

            var teacherId = requestInputs.GetValue("TeacherId", default(long?));
            if (teacherId.HasValue && teacherId.Value > 0)
                query = query.Where(item => item.TeacherId == teacherId.Value);

            return query;
        }



    }
}
