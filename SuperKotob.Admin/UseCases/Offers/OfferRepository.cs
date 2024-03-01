using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Offers
{
  public  class OfferRepository : BaseRepository<Offer>
    {
        protected override IQueryable<Offer> SetInculdes(DbSet<Offer> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Course)
                        .Include(item=> item.Track)
                        .Include(item=> item.OfferCountries);
        }
        protected override IQueryable<Offer> SetWhere(IQueryable<Offer> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var courseId = requestInputs.GetValue("CourseId", default(long?));
            var trackId = requestInputs.GetValue("TrackId", default(long?));
            if (courseId.HasValue && courseId.Value > 0)
                query = query.Where(item => item.CourseId == courseId.Value);

            if (trackId.HasValue && trackId.Value > 0)
                query = query.Where(item => item.TrackId == trackId.Value);
            return query;
        }
    }
}
