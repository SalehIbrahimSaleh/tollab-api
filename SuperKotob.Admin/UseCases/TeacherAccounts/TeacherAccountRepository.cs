using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace Tollab.Admin.UseCases.TeacherAccounts
{
  public  class TeacherAccountRepository : BaseRepository<TeacherAccount>
    {
        protected override IQueryable<TeacherAccount> SetInculdes(DbSet<TeacherAccount> dbSet, RequestInputs requestInputs)
        {
            return dbSet.Include(item => item.Teacher);
        }
        protected override IQueryable<TeacherAccount> SetWhere(IQueryable<TeacherAccount> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
       

            var teacherId = requestInputs.GetValue("TeacherId", default(long?));
            if (teacherId.HasValue && teacherId.Value > 0)
                query = query.Where(item => item.TeacherId == teacherId.Value);

            return query;
        }
    }
}
