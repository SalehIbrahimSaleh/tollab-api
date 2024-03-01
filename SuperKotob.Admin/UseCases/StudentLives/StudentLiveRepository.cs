using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentLives
{
   public class StudentLiveRepository : BaseRepository<StudentLive>
    {
        protected override IQueryable<StudentLive> SetInculdes(DbSet<StudentLive> dbSet, RequestInputs requestInputs)
    {
        return dbSet.Include(item => item.Student).Include(item => item.Live);
    }
    protected override IQueryable<StudentLive> SetWhere(IQueryable<StudentLive> query, RequestInputs requestInputs)
    {
        query = base.SetWhere(query, requestInputs);
        var liveId = requestInputs.GetValue("LiveId", default(long?));
        if (liveId.HasValue && liveId.Value > 0)
            query = query.Where(item => item.LiveId == liveId.Value);

        var studentId = requestInputs.GetValue("StudentId", default(long?));
        if (studentId.HasValue && studentId.Value > 0)
            query = query.Where(item => item.StudentId == studentId.Value);

        return query;
    }
}
}
