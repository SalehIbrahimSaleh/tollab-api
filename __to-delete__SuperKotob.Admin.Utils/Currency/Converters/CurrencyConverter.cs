using System.Threading.Tasks;
using SuperKotob.Admin.Models;

namespace SuperKotob.Admin.Utils.Currency
{
    public class CurrencyConverter : ICurrencyConverter
    {
        public CurrencyName From
        {
            get; private set;
        }

        public CurrencyName To
        {
            get; private set;
        }
        public ICurrencyConvertService CurrencyService { get; private set; }


        public CurrencyConverter(ICurrencyConvertService currencyService, CurrencyName from, CurrencyName to)
        {
            this.From = from;
            this.To = to;
            this.CurrencyService = currencyService;
        }

        
        public async Task<CurrencyAmountResponse> ConvertAsync(decimal amount)
        {
            var response = await CurrencyService.ConvertAsync(new CurrencyConvertRequest()
            {
                From = From,
                To = To,
                Amount = amount
            });
            return response;
        }
    }
}
