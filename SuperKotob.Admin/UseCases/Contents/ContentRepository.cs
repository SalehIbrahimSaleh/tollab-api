using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Contents
{
   public class ContentRepository : BaseRepository<Content>
    {
        protected override IQueryable<Content> SetInculdes(DbSet<Content> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Group).Include(item => item.ContentType);
        }
        protected override IQueryable<Content> SetWhere(IQueryable<Content> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var groupId = requestInputs.GetValue("GroupId", default(long?));
            if (groupId.HasValue && groupId.Value > 0)
                query = query.Where(item => item.GroupId == groupId.Value);

            var contentTypeId = requestInputs.GetValue("ContentTypeId", default(long?));
            if (contentTypeId.HasValue && contentTypeId.Value > 0)
                query = query.Where(item => item.ContentTypeId == contentTypeId.Value);

            return query;
        }
        protected override IList<Content> PagedQuery(IQueryable<Content> query, PagingMeta paging)
        {

            var list = query
              .OrderBy(item => item.OrderNumber)
              .Skip(paging.Skip)
              .Take(paging.PageSize)
              .ToList();

            //
            return list;


        }
    }
}
