using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Sms
{
    public class SmsServiceResponseItem
    {
        public string ClientRef { get; set; }
        public string ErrorText { get; set; }
        public string MessageId { get; set; }
        public string MessagePrice { get; set; }
        public string Network { get; set; }
        public string RemainingBalance { get; set; }
        public string Status { get; set; }
        public string To { get; set; }
    }
}
