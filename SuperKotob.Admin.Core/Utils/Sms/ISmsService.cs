using SuperKotob.Admin.Utils.Sms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils
{
    public interface ISmsService
    {
        Task<SmsServiceResponse> SendAsync(SmsMessage message);
    }
}
