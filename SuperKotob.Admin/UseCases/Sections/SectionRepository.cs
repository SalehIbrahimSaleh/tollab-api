using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Sections
{
   public class SectionRepository : BaseRepository<Section>
    {
        protected override IQueryable<Section> SetInculdes(DbSet<Section> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Country);
        }
        protected override IQueryable<Section> SetWhere(IQueryable<Section> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var countryId = requestInputs.GetValue("CountryId", default(long?));
            if (countryId.HasValue && countryId.Value > 0)
                query = query.Where(item => item.CountryId == countryId.Value);

            //var isType = requestInputs.GetValue("isType", default(string));
            //if (isType == "1")
            //    query = query.Where(item => item.Type == 1);
            //if (isType == "2")
            //    query = query.Where(item => item.Type == 2);
            return query;
        }



    }
}
