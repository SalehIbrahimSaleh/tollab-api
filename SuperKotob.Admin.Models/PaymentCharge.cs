using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Models
{
    public class PaymentCharge
    {
        public int Amount { get; set; }
        public int CapturedAmount { get; set; }
        public PaymentCreditCard Card { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string FailureCode { get; set; }
        public string FailureReason { get; set; }
        public string Id { get; set; }
        public string Ip { get; set; }
        public string Object { get; set; }
        public int RefundedAmount { get; set; }
        public string State { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
