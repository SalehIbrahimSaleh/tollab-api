using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Models
{
    public class PaymentCustomer
    {
        public IList<PaymentCreditCard> Cards { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DefaultCardId { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Object { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
