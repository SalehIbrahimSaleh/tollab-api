using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tollab.Admin.UseCases.TopCourse
{
   public class TopCoursesRepository : BaseRepository<Data.Models.Views.TopCourses>
    {
        protected override IQueryable<Data.Models.Views.TopCourses> SetInculdes(DbSet<Data.Models.Views.TopCourses> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Country);
        }
        protected override IQueryable<Data.Models.Views.TopCourses> SetWhere(IQueryable<Data.Models.Views.TopCourses> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            long id = requestInputs.GetValue("CountryId", 0);
            query = query.Where(item => item.CountryId == id);


            var countryId = requestInputs.GetValue("CountryId", default(long?));
            if (countryId.HasValue && countryId.Value > 0)
                query = query.Where(item => item.CountryId == countryId.Value);


            return query;
        }

        public override async Task<DataResponse<Data.Models.Views.TopCourses>> GetAsync(long id)
        {
            if (id < 1)
                return new DataResponse<Data.Models.Views.TopCourses>((Data.Models.Views.TopCourses)null, "Id should be more than zero");

            var requestInputs = new RequestInputs(new Dictionary<string, string>()
            {
                ["countryId"] = id.ToString()
            });

            return await GetAsync(requestInputs);
        }

        public override async Task<DataResponse<Data.Models.Views.TopCourses>> GetAsync(RequestInputs requestInputs = null)
        {
            long page = requestInputs.GetValue("page", 1);

            var response = new DataResponse<Data.Models.Views.TopCourses>();
            var dbSet = DataContext.Set<Data.Models.Views.TopCourses>();
            var query = SetInculdes(dbSet, requestInputs);
            query = SetWhere(query, requestInputs);
            var orderedQuery = SetOrdering(query, requestInputs);

            var count = query.Count();
            response.Paging = new PagingMeta(page, count, 25);

            response.Data = PagedQuery(query, response.Paging);

            return response;
        }

        protected override IOrderedQueryable<Data.Models.Views.TopCourses> SetOrdering(IQueryable<Data.Models.Views.TopCourses> query, RequestInputs requestInputs)
        {
            var orderedQuery = query.OrderByDescending(item => item.Amount);
            return orderedQuery;
        }
        protected override IList<Data.Models.Views.TopCourses> PagedQuery(IQueryable<Data.Models.Views.TopCourses> query, PagingMeta paging)
        {

            var list = query
              .OrderByDescending(item => item.Amount)
              .Skip(paging.Skip)
              .Take(paging.PageSize)
              .ToList();

            return list;


        }

    }
}
