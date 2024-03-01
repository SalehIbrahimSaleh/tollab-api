using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Replies
{
   public class ReplyRepository : BaseRepository<Reply>
    {
        protected override IQueryable<Reply> SetInculdes(DbSet<Reply> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Student).Include(item=>item.Teacher).Include(item=>item.VideoQuestion);
        }
        protected override IQueryable<Reply> SetWhere(IQueryable<Reply> query, RequestInputs requestInputs)
        {
            var id = requestInputs.GetValue("id", default(long?));
            query = base.SetWhere(query, requestInputs);
            if (id == null)
            {
                var isLiveReply = requestInputs.GetValue("IsLiveReplies", default(int?));

                if (isLiveReply != null)
                {
                    query = query.Where(a => a.VideoQuestion.LiveId != null);
                    requestInputs.Remove("IsLiveReplies");
                }
                else
                {
                    query = query.Where(a => a.VideoQuestion.ContentId != null);
                }
            }
            
            var studentId = requestInputs.GetValue("StudentId", default(long?));
            if (studentId.HasValue && studentId.Value > 0)
                query = query.Where(item => item.StudentId == studentId.Value);

            var teacherId = requestInputs.GetValue("TeacherId", default(long?));
            if (teacherId.HasValue && teacherId.Value > 0)
                query = query.Where(item => item.TeacherId == teacherId.Value);


            var videoQuestionId = requestInputs.GetValue("VideoQuestionId", default(long?));
            if (videoQuestionId.HasValue && videoQuestionId.Value > 0)
                query = query.Where(item => item.VideoQuestionId == videoQuestionId.Value);
        
            return query;
        }
    }
}
