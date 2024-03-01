using SuperKotob.Admin.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        public string GetConnectionString(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("Connection string value cannot be read, provided key is empty or null");

            var connectionStringObject = ConfigurationManager.ConnectionStrings[key];
            if (connectionStringObject == null)
                return null;

            return connectionStringObject.ConnectionString;
        }

        public T GetValue<T>(string key, T defaultValue)
        {

            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException("Configuration value cannot be read, provided key is empty or null");

            var valueAsString = ConfigurationManager.AppSettings[key];
            return ConvertOrDefaultValue(valueAsString, defaultValue);
        }
        public T GetValueOrException<T>(string key)
        {
            var valueAsString = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(valueAsString))
                throw new ConfigurationErrorsException($"AppSettings has no key named '{key}' ");

            return GetValue<T>(key, default(T));
        }

        private static T ConvertOrDefaultValue<T>(string valueAsString, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(valueAsString))
                return defaultValue;


            var valueObject = Convert.ChangeType(valueAsString, typeof(T));
            var value = (T)valueObject;
            return value;
        }
    }
}
