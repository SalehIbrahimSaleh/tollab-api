using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.ExamQuestions
{
   public class ExamQuestionRepository : BaseRepository<ExamQuestion>
    {
        protected override IQueryable<ExamQuestion> SetInculdes(DbSet<ExamQuestion> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.ExamQuestionType).Include(item => item.Exam);
        }
        protected override IQueryable<ExamQuestion> SetWhere(IQueryable<ExamQuestion> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var ExamQuestionTypeId = requestInputs.GetValue("ExamQuestionTypeId", default(long?));
            if (ExamQuestionTypeId.HasValue && ExamQuestionTypeId.Value > 0)
                query = query.Where(item => item.ExamQuestionTypeId == ExamQuestionTypeId.Value);

            var ExamId = requestInputs.GetValue("ExamId", default(long?));
            if (ExamId.HasValue && ExamId.Value > 0)
                query = query.Where(item => item.ExamId == ExamId.Value);
            return query;
        }
    }
}
