using SuperKotob.Admin.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tollab.Admin.Data.Models
{
    [Table("CowPayLog")]
    public class CowPayLog : DataModel
    {
      
        [MaxLength]
        [Display(Name = "cowpay reference id")]
        public string cowpay_reference_id { get; set; }
        [MaxLength]
        [Display(Name = "merchant reference id")]
        public string merchant_reference_id { get; set; }
        [MaxLength]
        [Display(Name = "order status")]
        public string order_status { get; set; }
        [MaxLength]
        [Display(Name = "amount")]
        public string amount { get; set; }
        [MaxLength]
        [Display(Name = "signature")]
        public string signature { get; set; }
        [MaxLength]
        [Display(Name = "callback type")]
        public string callback_type { get; set; }
        [MaxLength]
        [Display(Name = "customer merchant profile id")]
        public string customer_merchant_profile_id { get; set; }
        [MaxLength]
        [Display(Name = "payment gateway reference id")]
        public string payment_gateway_reference_id { get; set; }
        [MaxLength]
        [Display(Name = "merchant code")]
        public string merchant_code { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime? CreationDate { get; set; }
    }
}
