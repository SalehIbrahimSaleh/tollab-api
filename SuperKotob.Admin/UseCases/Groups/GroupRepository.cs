using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Groups
{
   public class GroupRepository : BaseRepository<Group>
    {
        protected override IQueryable<Group> SetInculdes(DbSet<Group> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Course);
        }
        protected override IQueryable<Group> SetWhere(IQueryable<Group> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var courseId = requestInputs.GetValue("CourseId", default(long?));
            if (courseId.HasValue && courseId.Value > 0)
                query = query.Where(item => item.CourseId == courseId.Value);
            return query;
        }

        protected override IList<Group> PagedQuery(IQueryable<Group> query, PagingMeta paging)
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
