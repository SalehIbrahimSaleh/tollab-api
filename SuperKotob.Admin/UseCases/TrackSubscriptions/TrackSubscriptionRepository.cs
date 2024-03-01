using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.TrackSubscriptions
{
    public class TrackSubscriptionRepository : BaseRepository<TrackSubscription>
    {
        protected override IQueryable<TrackSubscription> SetInculdes(DbSet<TrackSubscription> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Track).Include(item => item.Student);
        }
        protected override IQueryable<TrackSubscription> SetWhere(IQueryable<TrackSubscription> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var TrackId = requestInputs.GetValue("TrackId", default(long?));
            if (TrackId.HasValue && TrackId.Value > 0)
                query = query.Where(item => item.TrackId == TrackId.Value);

            var StudentId = requestInputs.GetValue("StudentId", default(long?));
            if (StudentId.HasValue && StudentId.Value > 0)
                query = query.Where(item => item.StudentId == StudentId.Value);

            return query;
        }

    }

}
