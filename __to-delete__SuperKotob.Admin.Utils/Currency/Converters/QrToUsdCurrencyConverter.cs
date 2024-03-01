using SuperKotob.Admin.Models;

namespace SuperKotob.Admin.Utils.Currency
{
    public class QrToUsdCurrencyConverter : CurrencyConverter
    { 
        public QrToUsdCurrencyConverter(ICurrencyConvertService currencyService)
            : base(currencyService, CurrencyName.QR, CurrencyName.USD)
        {
        }
       
    }
}
