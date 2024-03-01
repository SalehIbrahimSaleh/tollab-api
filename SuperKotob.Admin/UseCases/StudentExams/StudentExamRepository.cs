using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentExams
{
   public class StudentExamRepository : BaseRepository<StudentExam>
    {
        protected override IQueryable<StudentExam> SetInculdes(DbSet<StudentExam> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Exam).Include(item => item.Student).Include(item=>item.SolveStatus).Include(item => item.TeacherAssistant);
        }
        protected override IQueryable<StudentExam> SetWhere(IQueryable<StudentExam> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var ExamId = requestInputs.GetValue("ExamId", default(long?));
            if (ExamId.HasValue && ExamId.Value > 0)
                query = query.Where(item => item.ExamId == ExamId.Value);

            var StudentId = requestInputs.GetValue("StudentId", default(long?));
            if (StudentId.HasValue && StudentId.Value > 0)
                query = query.Where(item => item.StudentId == StudentId.Value);

            var SolveStatusId = requestInputs.GetValue("SolveStatusId", default(long?));
            if (SolveStatusId.HasValue && SolveStatusId.Value > 0)
                query = query.Where(item => item.SolveStatusId == SolveStatusId.Value);
            var TeacherAssistantId = requestInputs.GetValue("TeacherAssistantId", default(long?));
            if (TeacherAssistantId.HasValue && TeacherAssistantId.Value > 0)
                query = query.Where(item => item.TeacherAssistantId == TeacherAssistantId.Value);

            return query;
        }
    }
}
