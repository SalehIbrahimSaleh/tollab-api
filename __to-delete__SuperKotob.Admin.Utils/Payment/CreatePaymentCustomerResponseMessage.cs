using SuperKotob.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Payment
{
    public class CreatePaymentCustomerResponseMessage
    {
        public PaymentCustomer Customer { get; set; }
        public PaymentError Error { get; set; }
        public bool HasError { get; set; }
    }
}
