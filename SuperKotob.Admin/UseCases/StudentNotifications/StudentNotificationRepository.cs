using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentNotifications
{
    class StudentNotificationRepository : BaseRepository<StudentNotification>
    {
        protected override IQueryable<StudentNotification> SetInculdes(DbSet<StudentNotification> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Student).Include(item => item.Reference);
        }
        protected override IQueryable<StudentNotification> SetWhere(IQueryable<StudentNotification> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var referenceId = requestInputs.GetValue("ReferenceId", default(long?));
            if (referenceId.HasValue && referenceId.Value > 0)
                query = query.Where(item => item.ReferenceId == referenceId.Value);

            var studentId = requestInputs.GetValue("StudentId", default(long?));
            if (studentId.HasValue && studentId.Value > 0)
                query = query.Where(item => item.StudentId == studentId.Value);

            return query;
        }
    }
}
