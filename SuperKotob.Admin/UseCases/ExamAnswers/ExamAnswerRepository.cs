using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.ExamAnswers
{
   public class ExamAnswerRepository : BaseRepository<ExamAnswer>
    {
        protected override IQueryable<ExamAnswer> SetInculdes(DbSet<ExamAnswer> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.ExamQuestion);
        }
        protected override IQueryable<ExamAnswer> SetWhere(IQueryable<ExamAnswer> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var ExamQuestionId = requestInputs.GetValue("ExamQuestionId", default(long?));
            if (ExamQuestionId.HasValue && ExamQuestionId.Value > 0)
                query = query.Where(item => item.ExamQuestionId == ExamQuestionId.Value);

             return query;
        }
    }
}
