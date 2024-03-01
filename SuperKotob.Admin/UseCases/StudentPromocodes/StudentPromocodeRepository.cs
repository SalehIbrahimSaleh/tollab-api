using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.StudentPromocodes
{
   public class StudentPromocodeRepository: BaseRepository<StudentPromoCode>
    {
        protected override IQueryable<StudentPromoCode> SetInculdes(DbSet<StudentPromoCode> dbSet, RequestInputs requestInputs)
    {
        return dbSet.Include(item => item.Student).Include(item => item.PromoCode);
    }
    protected override IQueryable<StudentPromoCode> SetWhere(IQueryable<StudentPromoCode> query, RequestInputs requestInputs)
    {
        query = base.SetWhere(query, requestInputs);
        var promoCodeId = requestInputs.GetValue("PromoCodeId", default(long?));
        if (promoCodeId.HasValue && promoCodeId.Value > 0)
            query = query.Where(item => item.PromoCodeId == promoCodeId.Value);

        var studentId = requestInputs.GetValue("StudentId", default(long?));
        if (studentId.HasValue && studentId.Value > 0)
            query = query.Where(item => item.StudentId == studentId.Value);

        return query;
    }
}
}
