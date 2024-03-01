using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Models
{
    public class PaymentCreditCard
    {
        public string Brand { get; set; }
        public int Cvc { get; set; }
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }
        public string Id { get; set; }
        public string Last4 { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public string Object { get; set; }
        public string CustomerId { get; set; }
    }
}
