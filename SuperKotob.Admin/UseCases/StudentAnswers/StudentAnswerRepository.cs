using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentAnswers
{
   public class StudentAnswerRepository : BaseRepository<StudentAnswer>
    {
        protected override IQueryable<StudentAnswer> SetInculdes(DbSet<StudentAnswer> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.ExamAnswer).Include(item => item.StudentExam).Include(item => item.ExamQuestion);
        }
        protected override IQueryable<StudentAnswer> SetWhere(IQueryable<StudentAnswer> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var ExamQuestionId = requestInputs.GetValue("ExamQuestionId", default(long?));
            if (ExamQuestionId.HasValue && ExamQuestionId.Value > 0)
                query = query.Where(item => item.ExamQuestionId == ExamQuestionId.Value);

            var ExamAnswerId = requestInputs.GetValue("ExamAnswerId", default(long?));
            if (ExamAnswerId.HasValue && ExamAnswerId.Value > 0)
                query = query.Where(item => item.ExamAnswerId == ExamAnswerId.Value);

            var StudentExamId = requestInputs.GetValue("StudentExamId", default(long?));
            if (StudentExamId.HasValue && StudentExamId.Value > 0)
                query = query.Where(item => item.StudentExamId == StudentExamId.Value);


            return query;
        }
    }
}
