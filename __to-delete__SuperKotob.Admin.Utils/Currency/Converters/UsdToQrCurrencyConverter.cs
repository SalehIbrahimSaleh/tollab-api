using SuperKotob.Admin.Models;

namespace SuperKotob.Admin.Utils.Currency
{
    public class UsdToQrCurrencyConverter : CurrencyConverter
    {

        public UsdToQrCurrencyConverter(ICurrencyConvertService currencyService)
            : base(currencyService, CurrencyName.USD, CurrencyName.QR)
        {
        }
    }
}
