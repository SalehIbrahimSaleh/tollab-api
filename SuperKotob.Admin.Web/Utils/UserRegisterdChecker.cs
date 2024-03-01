using SuperKotob.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modeer.Admin.Web.Utils
{
    public class UserRegisterdChecker
    {
        //public static bool CheckUserRelatedWithAnyProfiles(long? UserId)
        //{
        //    OrthoContext context = new OrthoContext();
        //    var CustomersUserIds = context.Customers.Where(item => item.UserId == UserId).Select(item => item.UserId).ToList();
        //    var TechniciansUserIds = context.Technicians.Where(item => item.UserId == UserId).Select(item => item.UserId).ToList();
        //    if (CustomersUserIds.Contains(UserId) || TechniciansUserIds.Contains(UserId))
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        //public static bool CheckEmailRelatedWithAnyProfiles(string Email)
        //{
        //    OrthoContext context = new OrthoContext();
        //    var CustomersEmail = context.Customers.Where(item=>item.Email==Email).Select(item => item.Email).ToList();
        //    var TechniciansEmail = context.Technicians.Where(item => item.Email == Email).Select(item => item.Email).ToList();
        //    if (CustomersEmail.Contains(Email) || TechniciansEmail.Contains(Email))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public static bool CheckSSNRelatedWithAnyProfiles(string NationalId)
        //{
        //    OrthoContext context = new OrthoContext();
        //    var TechniciansNational = context.Technicians.Where(item => item.NationalId == NationalId).Select(item => item.NationalId).ToList();
        //    if ( TechniciansNational.Contains(NationalId))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

    }
}