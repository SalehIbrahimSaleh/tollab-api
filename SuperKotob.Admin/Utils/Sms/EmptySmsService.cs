using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Sms
{
    public class EmptySmsService : ISmsService
    {
        public async Task<SmsServiceResponse> SendAsync(SmsMessage message)
        {
            return new SmsServiceResponse()
            {
                Items = new List<SmsServiceResponseItem>()
                {
                    new SmsServiceResponseItem()
                }
            };
        }
    }
}
