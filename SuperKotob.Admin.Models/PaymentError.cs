using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Models
{
    public class PaymentError
    {
        public string Code { get; set; }
     
        public string Extras { get; set; }
       
        public string Message { get; set; }
       
        public string Type { get; set; }
    }
}
