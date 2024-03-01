using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.TrackPromotions
{
   public class TrackPromotionRepository:BaseRepository<TrackPromotion>
    {
        protected override IQueryable<TrackPromotion> SetInculdes(DbSet<TrackPromotion> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Track);
        }
        protected override IQueryable<TrackPromotion> SetWhere(IQueryable<TrackPromotion> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var trackId = requestInputs.GetValue("TrackId", default(long?));
            if (trackId.HasValue && trackId.Value > 0)
                query = query.Where(item => item.TrackId == trackId.Value);

            return query;
        }

    }
}
