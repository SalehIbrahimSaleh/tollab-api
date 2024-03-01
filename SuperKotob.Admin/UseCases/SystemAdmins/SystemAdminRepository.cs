using Tollab.Admin.Data.Models;
using SuperKotob.Admin.Core;
using SuperKotob.Admin.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tollab.Admin.Data.Models;

namespace OrthoHouse.Admin.UseCases.SystemAdmins
{
   public class SystemAdminRepository : BaseRepository<SystemAdmin>
    {
        //protected override IQueryable<SystemAdmin> SetInculdes(DbSet<SystemAdmin> dbSet, RequestInputs requestInputs)
        //{
        //    return dbSet.Include(item => item.Doctor).Include(item => item.Employee);
        //}
        protected override IQueryable<SystemAdmin> SetWhere(IQueryable<SystemAdmin> query, RequestInputs requestInputs)
        {
            query = base.SetWhere(query, requestInputs);
            var isType = requestInputs.GetValue("isType", default(string));
            if (isType == "1")
                query = query.Where(item => item.Type == 1);
            if (isType == "2")
                query = query.Where(item => item.Type == 2);
            return query;
        }



    }
}
