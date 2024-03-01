using SuperKotob.Admin.Data;
using SuperMatjar.WebProtector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Modeer.Admin.Web.Utils
{
    public class CheckMobileForRegister
    {
        TollabContext Context = new TollabContext();
        ProtectorDbContext db = new ProtectorDbContext();


        public bool checkIfRegistered(string Phone)
        {
            //var PhoneEmployeeList = Context.Employees.Where(item => item.PhoneNumber == Phone).Select(item => item.PhoneNumber).FirstOrDefault();
            //var PhoneDoctorList = Context.Doctors.Where(item => item.PhoneNumber == Phone).Select(item => item.PhoneNumber).FirstOrDefault();
            //var Usersphone = db.Users.Where(item => item.PhoneNumber == Phone).Select(b => b.PhoneNumber).FirstOrDefault();

            if (false)
            {
                return false;
            }
            return true;
        }
    }
}