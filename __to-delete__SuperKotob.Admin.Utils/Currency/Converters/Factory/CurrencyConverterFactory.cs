using SuperKotob.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Currency
{
    public class CurrencyConverterFactory : ICurrencyConverterFactory
    {
        ICurrencyConverter DefaultConverter { get; set; }
        ICurrencyConverter DefaultConverterReversed { get; set; }

        public CurrencyConverterFactory(ICurrencyConverter defaultConverter, ICurrencyConverter defaultConverterReversed)
        {
            this.DefaultConverter = defaultConverter;
            this.DefaultConverterReversed = defaultConverterReversed;
        }


        public ICurrencyConverter Get(CurrencyName from, CurrencyName to)
        {
            throw new NotImplementedException();
        }

        public ICurrencyConverter GetDefault()
        {
            return DefaultConverter;
        }

        public ICurrencyConverter GetDefaultReversed()
        {
            return DefaultConverterReversed;
        }
    }
}
