using SuperKotob.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Currency
{
    public class CurrencyConvertRequest
    {
        public CurrencyName From { get; set; }
        public CurrencyName To { get; set; }
        public decimal Amount { get; set; }
    }
}
