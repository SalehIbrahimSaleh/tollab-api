using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.LiveAttachments
{
    class LiveAttachmentRepository : BaseRepository<LiveAttachment>
    {
        protected override IQueryable<LiveAttachment> SetInculdes(DbSet<LiveAttachment> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Live);
        }
        protected override IQueryable<LiveAttachment> SetWhere(IQueryable<LiveAttachment> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var LiveId = requestInputs.GetValue("LiveId", default(long?));
            if (LiveId.HasValue && LiveId.Value > 0)
                query = query.Where(item => item.LiveId == LiveId.Value);
            
            return query;
        }
    }
}
