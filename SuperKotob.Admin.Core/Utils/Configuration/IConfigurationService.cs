using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public interface IConfigurationService
    {
        T GetValueOrException<T>(string key);
        T GetValue<T>(string key, T defaultValue);
        string GetConnectionString(string key);
    }
}
