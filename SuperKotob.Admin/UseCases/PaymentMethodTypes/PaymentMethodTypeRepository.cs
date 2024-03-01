using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.PaymentMethodTypes
{
    public class PaymentMethodTypeRepository : BaseRepository<PaymentMethodType>
    {
        protected override IQueryable<PaymentMethodType> SetInculdes(DbSet<PaymentMethodType> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Country);
        }
        protected override IQueryable<PaymentMethodType> SetWhere(IQueryable<PaymentMethodType> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var countryId = requestInputs.GetValue("CountryId", default(long?));
            if (countryId.HasValue && countryId.Value > 0)
                query = query.Where(item => item.CountryId == countryId.Value);
            return query;
        }
    }

}
