using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Courses
{
 public   class CourseRepository : BaseRepository<Course>
    {
        protected override IQueryable<Course> SetInculdes(DbSet<Course> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Track).Include(item => item.CourseStatus);
        }
        protected override IQueryable<Course> SetWhere(IQueryable<Course> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var trackId = requestInputs.GetValue("TrackId", default(long?));
            if (trackId.HasValue && trackId.Value > 0)
                query = query.Where(item => item.TrackId == trackId.Value);

            var courseStatusId = requestInputs.GetValue("CourseStatusId", default(long?));
            if (courseStatusId.HasValue && courseStatusId.Value > 0)
                query = query.Where(item => item.CourseStatusId == courseStatusId.Value);
            return query;
        }

        protected override IList<Course> PagedQuery(IQueryable<Course> query, PagingMeta paging)
        {

            var list = query
              .OrderBy(item => item.OrderNumber)
              .Skip(paging.Skip)
              .Take(paging.PageSize)
              .ToList();

            return list;


        }


    }
}
