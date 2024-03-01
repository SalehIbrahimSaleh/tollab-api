using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentTransactions
{
   public class StudentTransactionRepository : BaseRepository<StudentTransaction>
    {
        protected override IQueryable<StudentTransaction> SetInculdes(DbSet<StudentTransaction> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Course).Include(item => item.Student).Include(item=>item.Track);
        }
        protected override IQueryable<StudentTransaction> SetWhere(IQueryable<StudentTransaction> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var courseId = requestInputs.GetValue("CourseId", default(long?));
            if (courseId.HasValue && courseId.Value > 0)
                query = query.Where(item => item.CourseId == courseId.Value);

            var studentId = requestInputs.GetValue("StudentId", default(long?));
            if (studentId.HasValue && studentId.Value > 0)
                query = query.Where(item => item.StudentId == studentId.Value);

            var trackId = requestInputs.GetValue("TrackId", default(long?));
            if (trackId.HasValue && trackId.Value > 0)
                query = query.Where(item => item.TrackId == trackId.Value);


            return query;
        }
    }
}
