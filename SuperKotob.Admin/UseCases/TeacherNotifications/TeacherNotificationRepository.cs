using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.TeacherNotifications
{
  public  class TeacherNotificationRepository : BaseRepository<TeacherNotification>
    {
        protected override IQueryable<TeacherNotification> SetInculdes(DbSet<TeacherNotification> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Reference).Include(item => item.Teacher);
        }
        protected override IQueryable<TeacherNotification> SetWhere(IQueryable<TeacherNotification> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var referenceId = requestInputs.GetValue("ReferenceId", default(long?));
            if (referenceId.HasValue && referenceId.Value > 0)
                query = query.Where(item => item.ReferenceId == referenceId.Value);

            var teacherId = requestInputs.GetValue("TeacherId", default(long?));
            if (teacherId.HasValue && teacherId.Value > 0)
                query = query.Where(item => item.TeacherId == teacherId.Value);

            return query;
        }
    }
}
