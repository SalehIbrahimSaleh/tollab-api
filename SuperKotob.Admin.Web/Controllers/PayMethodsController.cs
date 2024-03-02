using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperKotob.Admin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Tollab.Admin.Web.MyFatoorah;

namespace Tollab.Admin.Web.Controllers
{
    public class PayMethodsController : Controller
    {
        TollabContext db = new TollabContext();

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> DoPayment(string PaymentKey, double InvoiceValue)
        {
            if (string.IsNullOrEmpty(PaymentKey))
            {
                return View("Error");
            }
            var student = db.Students.Where(p => p.PaymentKey == PaymentKey).FirstOrDefault();
            if (student == null)
            {
                return View("Error");

            }
            string uri = "https://api.myfatoorah.com/v2/SendPayment";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", "nZ6ZCaIe1JbhVsbEdIPEISgty9KpEZtD-ojVm3GIo2PoE1EWSa-HibbYSwegi8sXUjt5vX_V36YVHPJmOE-8RrXcV7DPuOBE7bIfxbo4p_mu5c-gSqvhtbTaLaJ5hWKbCtAMHN01BGwci1Qax9sZrsN27ZDU_Lthj6toxVvlQOCVGPtAwfIG3XajawHT79f9tHwixTBpGDZnTXwRoCP6teiLiryyPMjasqugUBJpp6oOBlrW1d4HLV73h5tDJbLEeeLo_532c3ONCHLrzTnMCFZe3d8ZE5AgEYw_jf2pun1owXUCG-zlBOyyjCEp46VJ_-2wGDtG2aKWE4ooTtCFYGKQ_lbT6t4lCLvj_yCdi43khIwclP-Ug2CnMurld_9PiIfLWRN3F0MXepN7WLJBlR0Z5BUZPXguQGJ6DtqJx6Vmn598BFMTJyz43aT_lSqA7wG20-xh_e9jNBoEMYEDov4Zsw-nPJANQDzJVmxlaT76_VaRWyz8B3p3rtOmlP2zgwQjKqosq0iblbdIWWheBKJjmjl4Isq4cbrrm1pqevl31ykkWVd3gThaUXiLkUJNieGVv1VUoZlO3A53zHj-ievz_7vTlL5Kr34GrtjgFZ03jD1fJsPuYmX35Fj2aPXr6yiwwhHL85Gr3gvUFDQJ2VunMFmz1eBEC-JsKDajH4BwxPrv");
            var BodyModel = new
            {
                CustomerName = student.Id.ToString(),
                NotificationOption = "LNK",
                InvoiceValue = InvoiceValue,
                MobileCountryCode = student.PhoneKey,
                CustomerMobile = student.Phone,
                CustomerEmail = student.Email,
                CallBackUrl = "http://tollab.com/tollab/api/OnPaymentSuccess",
                ErrorUrl = "http://tollab.com/tollab/api/OnPaymentError",
                Language = "en",
                ExpiryDate = DateTime.UtcNow.AddDays(1)
            };
            var response2 = await client.PostAsJsonAsync(uri, BodyModel);
            var responseString = await response2.Content.ReadAsStringAsync();
            var tempResponse = JObject.Parse(responseString);
            responseString = tempResponse.ToString();
            var responseCode = response2.StatusCode;
            var BankResponseModel = JsonConvert.DeserializeObject<ResponseModdel>((JObject.Parse(responseString)).ToString());
            if (BankResponseModel.Data.InvoiceURL != null)
            {
                return RedirectPermanent(BankResponseModel.Data.InvoiceURL);
            }

            return View("Error");
        }

        public async System.Threading.Tasks.Task<ActionResult> DoPayment(string PaymentKey)
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


        public ActionResult BuyCourse(string PaymentKey)
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

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> BuyCourse(string PaymentKey, string CourseCode)
        {
            try
            {

                string message = "";
                if (string.IsNullOrEmpty(PaymentKey))
                {
                    return View("Error");
                }
                var student = db.Students.Where(p => p.PaymentKey == PaymentKey).FirstOrDefault();
                if (student == null)
                {
                    return View("Error");

                }
                string uri = "http://tollab.com/tollab/api/BuyCourseByLink?PaymentKey=" + PaymentKey + "&CourseCode=" +CourseCode+"&PromocodeText=";
                var client = new HttpClient();
                var response2 = await client.GetAsync(uri);
                var responseString = await response2.Content.ReadAsStringAsync();
                var tempResponse = JObject.Parse(responseString);
                responseString = tempResponse.ToString();
                var responseCode = response2.StatusCode;
                if (responseCode == HttpStatusCode.OK)
                {
                    return View("Success");
                }
                else if (responseCode==HttpStatusCode.BadRequest)
                {
                    message = (string)tempResponse["errors"]["message"];

                    return RedirectToAction
                   ("ErrorPage", new RouteValueDictionary(
                   new { controller = "PayMethods", action = "ErrorPage", message = message }));

                }
                return View("Error");
            }
            catch (Exception e)
            {

                return View("Error");
            }
        }



        public ActionResult BuyTrack(string PaymentKey)
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

        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> BuyTrack(string PaymentKey, string TrackCode)
        {
            try
            {

                string message = "";
                if (string.IsNullOrEmpty(PaymentKey))
                {
                    return View("Error");
                }
                var student = db.Students.Where(p => p.PaymentKey == PaymentKey).FirstOrDefault();
                if (student == null)
                {
                    return View("Error");

                }
                string uri = "http://tollab.com/tollab/api/TrackSubscriptionByLink?PaymentKey=" + PaymentKey + "&TrackCode=" + TrackCode + "&PromocodeText=";
                var client = new HttpClient();
                var response2 = await client.GetAsync(uri);
                var responseString = await response2.Content.ReadAsStringAsync();
                var tempResponse = JObject.Parse(responseString);
                responseString = tempResponse.ToString();
                var responseCode = response2.StatusCode;
                if (responseCode == HttpStatusCode.OK)
                {
                    return View("Success");
                }
                else if (responseCode == HttpStatusCode.BadRequest)
                {
                    message = (string)tempResponse["errors"]["message"];

                    return RedirectToAction
                   ("ErrorPage", new RouteValueDictionary(
                   new { controller = "PayMethods", action = "ErrorPage", message = message }));

                }
                return View("Error");
            }
            catch (Exception e)
            {

                return View("Error");
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult ErrorPage(string message)
        {
            ViewBag.Error = message;
            return View();
        }

    }
}