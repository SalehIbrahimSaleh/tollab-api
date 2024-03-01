using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public class ConfigurationConnectionStringAccessor : IConfigurationConnectionStringAccessor
    {
        public IConfigurationService ConfigurationService { get; private set; }

        public ConfigurationConnectionStringAccessor(
            IConfigurationService configurationService,
            string key)
        {
            this.ConfigurationService = configurationService;
            this.Key = key;
        }

        public string Key { get; private set; }

        bool IsValueInitialized { get; set; }

        string Value { get; set; }

        public async Task<string> GetValueAsync()
        {
            if (IsValueInitialized)
                return Value;

            Value = ConfigurationService.GetConnectionString(Key);
            IsValueInitialized = true;

            return Value;
        }
    }
}
