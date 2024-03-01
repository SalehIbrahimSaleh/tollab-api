using Microsoft.AspNet.Identity;
using SuperMatjar.WebProtector.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector
{
    internal static class ExtentionMethods
    {
        public static ProtectedUserResult ToProtectedUserResult(this IdentityResult identityResult)
        {
            var errors = identityResult.Errors == null ? null : identityResult.Errors.ToList();
            var result = new ProtectedUserResult(identityResult.Succeeded, errors);
            return result;
        }
    }
}
