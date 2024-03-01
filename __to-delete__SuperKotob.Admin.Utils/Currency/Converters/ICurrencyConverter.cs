using SuperKotob.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Currency
{
    public interface ICurrencyConverter
    {
        CurrencyName From { get; }
        CurrencyName To { get; }

        Task<CurrencyAmountResponse> ConvertAsync(decimal amount);
    }
}
