using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.SystemNotifications
{
   public class SystemNotificationRepository : BaseRepository<SystemNotification>
    {
        protected override IQueryable<SystemNotification> SetInculdes(DbSet<SystemNotification> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.NotificationType);
        }
        protected override IQueryable<SystemNotification> SetWhere(IQueryable<SystemNotification> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);

            var NotificationTypeId = requestInputs.GetValue("NotificationTypeId", default(long?));
            if (NotificationTypeId.HasValue && NotificationTypeId.Value > 0)
                query = query.Where(item => item.NotificationTypeId == NotificationTypeId.Value);

            return query;
        }
    }
}
