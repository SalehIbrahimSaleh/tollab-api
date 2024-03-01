using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Categories
{
   public class CategoryRepository : BaseRepository<Category>
    {
        protected override IQueryable<Category> SetInculdes(DbSet<Category> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Section);
        }
        protected override IQueryable<Category> SetWhere(IQueryable<Category> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var sectionId = requestInputs.GetValue("SectionId", default(long?));
            if (sectionId.HasValue && sectionId.Value > 0)
                query = query.Where(item => item.SectionId == sectionId.Value);

            //var isType = requestInputs.GetValue("isType", default(string));
            //if (isType == "1")
            //    query = query.Where(item => item.Type == 1);
            //if (isType == "2")
            //    query = query.Where(item => item.Type == 2);
            return query;
        }
    }
}
