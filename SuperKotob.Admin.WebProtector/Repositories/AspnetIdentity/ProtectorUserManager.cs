using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMatjar.WebProtector.Core;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.Owin;

namespace SuperMatjar.WebProtector
{
    public class ProtectorUserManager : UserManager<ProtectedUser>
    {
        public ProtectorUserManager(IUserStore<ProtectedUser> store)
            : base(store)
        {
            var provider = new DpapiDataProtectionProvider("SuperMatjar");
            var dataProtector = provider.Create("ASP.NET Identity");
            this.UserTokenProvider = new DataProtectorTokenProvider<ProtectedUser>(dataProtector);
          
        }


        public async Task<IdentityResult> CreateAsync(ProtectedUser user, string password)
        {
            try
            {
                return await base.CreateAsync(user, password);
            }
            catch (Exception ex)
            {
                var e = ex.GetBaseException();
                throw;
            }
        }
    }
}
