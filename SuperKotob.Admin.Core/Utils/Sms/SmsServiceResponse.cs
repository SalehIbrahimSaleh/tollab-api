using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Sms
{
    public class SmsServiceResponse
    {

        public IList<SmsServiceResponseItem> Items { get; set; } = new List<SmsServiceResponseItem>();
        public string Text { get; set; }
    }
}
