using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tollab.Admin.Web.MyFatoorah
{
    public class Data
    {
        public int InvoiceId { get; set; }
        public string InvoiceURL { get; set; }
        public object CustomerReference { get; set; }
        public object UserDefinedField { get; set; }
    }
}