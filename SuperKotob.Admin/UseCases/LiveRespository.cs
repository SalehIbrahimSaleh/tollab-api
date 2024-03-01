using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases
{
    public class LiveRespository : BaseRepository<Live>
    {
        protected override IQueryable<Live> SetInculdes(DbSet<Live> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Course)
                        .Include(item => item.Teacher)
                        .Include(item => item.Track)
                        .Include(item => item.Country);
        }
        protected override IQueryable<Live> SetWhere(IQueryable<Live> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var courseId = requestInputs.GetValue("CourseId", default(long?));
            var trackId = requestInputs.GetValue("TrackId", default(long?));
            var teacherID = requestInputs.GetValue("TeacherId", default(long?));
            if (courseId.HasValue && courseId.Value > 0)
                query = query.Where(item => item.CourseId == courseId.Value);

            if (trackId.HasValue && trackId.Value > 0)
                query = query.Where(item => item.TrackId == trackId.Value);

            if (teacherID.HasValue && teacherID.Value > 0)
                query = query.Where(item => item.TeacherId == teacherID.Value);
            return query;
        }
    }
}
