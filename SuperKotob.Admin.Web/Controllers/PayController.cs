using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperKotob.Admin.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tollab.Admin.Web.MyFatoorah;

namespace Tollab.Admin.Web.Controllers
{
    public class PayController : Controller
    {
        TollabContext db = new TollabContext();

        // GET: Pay
        public async Task<ActionResult> Index(string PaymentKey)
        {

     

            ViewBag.PaymentKey = PaymentKey;
            if (PaymentKey != null)
            {
                var student = db.Students.Where(p => p.PaymentKey == PaymentKey).FirstOrDefault();
                if (student != null)
                {
                    ViewBag.StudentName = student.Name;
                }
            }

            return View();
        }

        public string getCourseOrTrackCode()
        {

            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = alphabets + small_alphabets + numbers;

            int length = 4;

            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

    }
}




/*
 *      
 *      var tracks = db.Courses.Where(i=>i.CourseCode==null).ToList();
            foreach (var item in tracks)
            {
                try
                {
                    //add code to course
                    var code = getCourseOrTrackCode();
                    var TrackCode = "C" + item.Id + code;
                    item.CourseCode = TrackCode;
                    db.Entry(item).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                }
                catch (Exception e)
                {

                }
            }
 * */
