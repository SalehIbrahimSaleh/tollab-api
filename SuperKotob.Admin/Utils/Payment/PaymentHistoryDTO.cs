using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Payment
{
    public class PaymentHistoryDTO
    {
        public long OrderId { get; set; }
        public long CardTypeId { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
    }
}
