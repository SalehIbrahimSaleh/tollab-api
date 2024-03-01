using SuperKotob.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Currency
{
    public interface ICurrencyConverterFactory
    {
        ICurrencyConverter Get(CurrencyName from, CurrencyName to);
        ICurrencyConverter GetDefault();
        ICurrencyConverter GetDefaultReversed();

    }
}
