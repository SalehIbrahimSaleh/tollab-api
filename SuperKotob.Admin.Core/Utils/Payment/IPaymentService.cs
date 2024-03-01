using SuperKotob.Admin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Payment
{
    public interface IPaymentService
    {
        Task<ApiResponse<KeyValuePair<string, string>>> Purchase(RequestInputs requestInputs);

        Task<string> AfterPurchase(RequestInputs requestInputs);
        Task AfterCardRegistration(RequestInputs requestInputs);
    }
}
