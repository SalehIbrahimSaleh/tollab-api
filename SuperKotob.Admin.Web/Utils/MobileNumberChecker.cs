using SuperKotob.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Modeer.Admin.Web.Utils
{
    public class MobileNumberChecker
    {
        public static string handelMobileNumber(string PhoneNumber)
        {

            PhoneNumber = PhoneNumber.Trim();

            if (PhoneNumber.StartsWith("00"))
            {
                string toremove = PhoneNumber.Remove(0, 2);
                PhoneNumber = "+" + PhoneNumber;
            }
            if (PhoneNumber.StartsWith("01"))
            {
                PhoneNumber = "+2" + PhoneNumber;
            }
            if (!PhoneNumber.StartsWith("+"))
            {
             PhoneNumber="+" + PhoneNumber;
            }
            if (PhoneNumber.StartsWith("+2001"))
            {
                PhoneNumber= PhoneNumber.Remove(2, 1);
            }
            
            return PhoneNumber;

        }
       

    }
}