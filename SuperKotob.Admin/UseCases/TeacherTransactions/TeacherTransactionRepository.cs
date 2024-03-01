using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.TeacherTransactions
{
   public class TeacherTransactionRepository : BaseRepository<TeacherTransaction>
    {
        protected override IQueryable<TeacherTransaction> SetInculdes(DbSet<TeacherTransaction> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Course).Include(item => item.Teacher).Include(item => item.Track);
        }
        protected override IQueryable<TeacherTransaction> SetWhere(IQueryable<TeacherTransaction> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var courseId = requestInputs.GetValue("CourseId", default(long?));
            if (courseId.HasValue && courseId.Value > 0)
                query = query.Where(item => item.CourseId == courseId.Value);

            var teacherId = requestInputs.GetValue("TeacherId", default(long?));
            if (teacherId.HasValue && teacherId.Value > 0)
                query = query.Where(item => item.TeacherId == teacherId.Value);

            var trackId = requestInputs.GetValue("TrackId", default(long?));
            if (trackId.HasValue && trackId.Value > 0)
                query = query.Where(item => item.TrackId == trackId.Value);

            return query;
        }
    }
}
