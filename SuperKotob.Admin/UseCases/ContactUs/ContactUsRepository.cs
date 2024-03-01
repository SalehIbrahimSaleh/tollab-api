using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;
using ContactUsData = Tollab.Admin.Data.Models.ContactUs;

namespace Tollab.Admin.UseCases.ContactUs
{
   public class ContactUsRepository : BaseRepository<ContactUsData>
    {
        protected override IQueryable<ContactUsData> SetInculdes(DbSet<ContactUsData> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Student).Include(i=>i.Teacher);
        }
        protected override IQueryable<ContactUsData> SetWhere(IQueryable<ContactUsData> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var studentId = requestInputs.GetValue("StudentId", default(long?));
            if (studentId.HasValue && studentId.Value > 0)
                query = query.Where(item => item.StudentId == studentId.Value);
            var TeacherId = requestInputs.GetValue("TeacherId", default(long?));
            if (TeacherId.HasValue && TeacherId.Value > 0)
                query = query.Where(item => item.TeacherId == TeacherId.Value);
            return query;
        }
    }
}
