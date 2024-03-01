using StructureMap;
using SuperKotob.Admin.Utils.Currency;
using SuperKotob.Admin.Utils.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.DependencyManagement
{
    public class UtilsRegistry : Registry
    {
        public UtilsRegistry()
        {

            For<IHttpClient>().Use<SuperKotobHttpClient>().Singleton();
            For<ICurrencyConverterFactory>().Use<CurrencyConverterFactory>()
                .Ctor<ICurrencyConverter>("defaultConverter").Is<QrToUsdCurrencyConverter>()
                .Ctor<ICurrencyConverter>("defaultConverterReversed").Is<UsdToQrCurrencyConverter>();
        }
    }
}
