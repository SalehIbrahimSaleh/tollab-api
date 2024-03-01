using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Currency
{
    public interface ICurrencyConvertService
    {
        Task<CurrencyAmountResponse> ConvertAsync(CurrencyConvertRequest request);
    }
}
