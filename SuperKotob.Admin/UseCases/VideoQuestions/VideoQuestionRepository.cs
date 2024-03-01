using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.VideoQuestions
{
  public  class VideoQuestionRepository : BaseRepository<VideoQuestion>
    {
        protected override IQueryable<VideoQuestion> SetInculdes(DbSet<VideoQuestion> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Student)
                        .Include(item=> item.Live)
                        .Include(item => item.Content)
                        .Include(item => item.Replies);
        }
        protected override IQueryable<VideoQuestion> SetWhere(IQueryable<VideoQuestion> query, RequestInputs requestInputs)
        {
            var isLiveQuestion = requestInputs.GetValue<string>("isLiveQuestions", null);
            var studentId = requestInputs.GetValue("StudentId", default(long?));
            if (isLiveQuestion != null)
            {
                requestInputs.Remove("isLiveQuestion");
                query = base.SetWhere(query, requestInputs);

                query = query.Where(a => a.LiveId != null);

                if (studentId.HasValue && studentId.Value > 0)
                    query = query.Where(item => item.StudentId == studentId.Value);

                var LiveId = requestInputs.GetValue("LiveId", default(long?));
                if (LiveId.HasValue && LiveId.Value > 0)
                    query = query.Where(item => item.LiveId == LiveId.Value);

                return query;
            }
            query = base.SetWhere(query, requestInputs);
            query = query.Where(a => a.ContentId != null);
            var contentId = requestInputs.GetValue("ContentId", default(long?));
            if (contentId.HasValue && contentId.Value > 0)
                query = query.Where(item => item.ContentId == contentId.Value);


            if (studentId.HasValue && studentId.Value > 0)
                query = query.Where(item => item.StudentId == studentId.Value);

            return query;
        }
    }

}
