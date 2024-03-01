using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.Teachers
{
   public class TeacherRepository : BaseRepository<Teacher>
    {
        protected override IQueryable<Teacher> SetInculdes(DbSet<Teacher> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Country);
        }
        protected override IQueryable<Teacher> SetWhere(IQueryable<Teacher> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var countryId = requestInputs.GetValue("CountryId", default(long?));
            if (countryId.HasValue && countryId.Value > 0)
                query = query.Where(item => item.CountryId == countryId.Value);

            var isGender = requestInputs.GetValue("IsGender", default(string));
            if (isGender == "1")
                query = query.Where(item => item.Gender == true);
            if (isGender == "2")
                query = query.Where(item => item.Gender == false);

            var isEnabled = requestInputs.GetValue("IsEnabled", default(string));
            if (isEnabled == "1")
                query = query.Where(item => item.Enabled == true);
            if (isEnabled == "2")
                query = query.Where(item => item.Enabled == false);
            return query;
        }
    }
}
