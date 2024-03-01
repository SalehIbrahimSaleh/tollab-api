using SuperKotob.Admin.Core.Utils.Validation;
using SuperKotob.Admin.Utils;
using SuperKotob.Admin.Utils.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public class ConfigurationValueAccessor<T> : IConfigurationValueAccessor<T>
    {
        IConfigurationService ConfigurationService { get; set; }

        public ConfigurationValueAccessor(
            IConfigurationService configurationService,
            string key,
            T defaultValue,
            IValidator<T> validator = null)
        {
            this.ConfigurationService = configurationService;
            this.Key = key;
            this.DefaultValue = defaultValue;
            this.Validator = validator;

        }

        public async virtual Task<T> GetValueAsync()
        {
            if (IsValueInitialized)
                return Value;

            Value = ConfigurationService.GetValue(Key, DefaultValue);
            if (Validator != null)
            {
                var validationResult = await Validator.ValidateAsync(new ValidatorContext<T>(Key, Value));
                if (!validationResult.IsValid)
                {
                    throw new Exception("Configuration Error: " + string.Join(", ", validationResult.Messages));
                }
            }

            IsValueInitialized = true;
            return Value;
        }

        public string Key { get; private set; }


        T DefaultValue { get; set; }
        T Value { get; set; }
        bool IsValueInitialized { get; set; }
        public IValidator<T> Validator { get; private set; }
    }
}
