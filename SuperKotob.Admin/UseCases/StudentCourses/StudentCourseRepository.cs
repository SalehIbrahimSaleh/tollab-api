using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentCourses
{
   public class StudentPackageRepository : BaseRepository<StudentCourse>
    {
        protected override IQueryable<StudentCourse> SetInculdes(DbSet<StudentCourse> dbSet, RequestInputs requestInputs)
    {
        return dbSet.Include(item => item.Student).Include(item => item.Course);
    }
    protected override IQueryable<StudentCourse> SetWhere(IQueryable<StudentCourse> query, RequestInputs requestInputs)
    {
        query = base.SetWhere(query, requestInputs);
        var courseId = requestInputs.GetValue("CourseId", default(long?));
        if (courseId.HasValue && courseId.Value > 0)
            query = query.Where(item => item.CourseId == courseId.Value);

        var studentId = requestInputs.GetValue("StudentId", default(long?));
        if (studentId.HasValue && studentId.Value > 0)
            query = query.Where(item => item.StudentId == studentId.Value);

        return query;
    }
}
}
