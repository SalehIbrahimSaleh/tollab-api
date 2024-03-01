using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models.Views;

namespace Tollab.Admin.UseCases.StudentSubjects
{
  public  class StudentSubjectsRepository : BaseRepository<StudentSubject>
    {
        protected override IQueryable<StudentSubject> SetInculdes(DbSet<StudentSubject> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Department).Include(i => i.Student).Include(i => i.Subject);
        }
        protected override IQueryable<StudentSubject> SetWhere(IQueryable<StudentSubject> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var departmentId = requestInputs.GetValue("DepartmentId", default(long?));
            if (departmentId.HasValue && departmentId.Value > 0)
                query = query.Where(item => item.DepartmentId == departmentId.Value);

            var StudentId = requestInputs.GetValue("StudentId", default(long?));
            if (StudentId.HasValue && StudentId.Value > 0)
                query = query.Where(item => item.StudentId == StudentId.Value);

            var SubjectId = requestInputs.GetValue("SubjectId", default(long?));
            if (SubjectId.HasValue && SubjectId.Value > 0)
                query = query.Where(item => item.SubjectId == SubjectId.Value);
            return query;
        }

        public override async Task<DataResponse<StudentSubject>> GetAsync(RequestInputs requestInputs = null)
        {
            long page = requestInputs.GetValue("page", 1);

            var response = new DataResponse<StudentSubject>();
            var dbSet = DataContext.Set<StudentSubject>();
            var query = SetInculdes(dbSet, requestInputs);
            query = SetWhere(query, requestInputs);
            var orderedQuery = SetOrdering(query, requestInputs);

            var count = query.Count();
            response.Paging = new PagingMeta(page, count, 25);

            query = query.Where(i => i.Id > 0).Distinct();

            response.Data = PagedQuery(query, response.Paging);

            return response;
        }
        protected override IList<StudentSubject> PagedQuery(IQueryable<StudentSubject> query, PagingMeta paging)
        {

            var list = query
              .OrderByDescending(item => item.Id)
              .Skip(paging.Skip)
              .Take(paging.PageSize)
              .Distinct()
              .ToList();

            return list;


        }



    }
}
