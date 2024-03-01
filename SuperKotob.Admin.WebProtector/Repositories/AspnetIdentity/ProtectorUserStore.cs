using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector
{
    public class ProtectorUserStore : UserStore<ProtectedUser>
    {
        public ProtectorUserStore(DbContext context)
            : base(context)
        {
        }
    }
}
