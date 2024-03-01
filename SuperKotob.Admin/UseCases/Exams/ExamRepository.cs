using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Exams
{
  public  class ExamRepository : BaseRepository<Exam>
    {
        protected override IQueryable<Exam> SetInculdes(DbSet<Exam> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.ExamType).Include(item => item.Course);
        }
        protected override IQueryable<Exam> SetWhere(IQueryable<Exam> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var ExamTypeId = requestInputs.GetValue("ExamTypeId", default(long?));
            if (ExamTypeId.HasValue && ExamTypeId.Value > 0)
                query = query.Where(item => item.ExamTypeId == ExamTypeId.Value);

            var courseId = requestInputs.GetValue("CourseId", default(long?));
            if (courseId.HasValue && courseId.Value > 0)
                query = query.Where(item => item.CourseId == courseId.Value);
            return query;
        }
    }
}
