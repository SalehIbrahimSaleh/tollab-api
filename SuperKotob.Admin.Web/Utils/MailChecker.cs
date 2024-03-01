using SuperKotob.Admin.Data;
using SuperMatjar.WebProtector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modeer.Admin.Web.Utils
{
    public class MailChecker
    {

        public static bool CheckEmailAddressExist(string email)
        {
            TollabContext context = new TollabContext();
            ProtectorDbContext db = new ProtectorDbContext();
             var UsersEmail = db.Users.Where(b => b.Email == email).Select(b => b.Email).FirstOrDefault();
            if (UsersEmail!=null)
            {
                return true;
            }
            return false;
        }

    }
}