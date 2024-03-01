
using SuperKotob.Admin.Core.Utils.Validation;
using SuperKotob.Admin.Utils.DI;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public class ConfigurationValueAccessorFactory : IConfigurationValueAccessorFactory
    {
        public IDependencyContainerScope ContainerScope { get; private set; }

        public ConfigurationValueAccessorFactory(IDependencyContainerScope containerScope)
        {
            this.ContainerScope = containerScope;
        }

        public IConfigurationValueAccessor<T> CreateValueAccessor<T>(string key, T defaultValue, IValidator<T> validator = null)
        {
            var args = new Dictionary<string, object>()
            {
                ["key"] = key,
                ["defaultValue"] = defaultValue,
                ["validator"] = validator
            };

            var accessor = ContainerScope.GetInstance<ConfigurationValueAccessor<T>>(args);
            return accessor;
        }

        public IConfigurationConnectionStringAccessor CreateConnectionStringAccessor(string key)
        {
            var args = new Dictionary<string, object>()
            {
                ["key"] = key
            };

            var accessor = ContainerScope.GetInstance<ConfigurationConnectionStringAccessor>(args);
            return accessor;
        }

    }
}
