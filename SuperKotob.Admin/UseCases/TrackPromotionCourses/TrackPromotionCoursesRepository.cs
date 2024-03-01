using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.TrackPromotionCourses
{
   public class TrackPromotionCourseRepository : BaseRepository<TrackPromotionCourse>
    {
        protected override IQueryable<TrackPromotionCourse> SetInculdes(DbSet<TrackPromotionCourse> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.TrackPromotion).Include(item=>item.Course);
        }
        protected override IQueryable<TrackPromotionCourse> SetWhere(IQueryable<TrackPromotionCourse> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            //var trackId = requestInputs.GetValue("TrackId", default(long?));
            //if (trackId.HasValue && trackId.Value > 0)
            //    query = query.Where(item => item.TrackId == trackId.Value);

            return query;
        }

    }
}
