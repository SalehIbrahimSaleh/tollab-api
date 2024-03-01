using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.SubCategories
{
   public class SubCategoryRepository : BaseRepository<SubCategory>
    {
        protected override IQueryable<SubCategory> SetInculdes(DbSet<SubCategory> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Category);
        }
        protected override IQueryable<SubCategory> SetWhere(IQueryable<SubCategory> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var categoryId = requestInputs.GetValue("CategoryId", default(long?));
            if (categoryId.HasValue && categoryId.Value > 0)
                query = query.Where(item => item.CategoryId == categoryId.Value);

            return query;
        }



    }
}
