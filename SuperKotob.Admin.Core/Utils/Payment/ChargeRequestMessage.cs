using SuperKotob.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Payment
{
    public class ChargeRequestMessage
    {
        //public CurrencyAmount Amount { get; set; }
        public PaymentCreditCard CreditCard { get; set; }
    }
}
