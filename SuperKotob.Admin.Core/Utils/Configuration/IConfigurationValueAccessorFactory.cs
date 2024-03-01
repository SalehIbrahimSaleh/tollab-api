using SuperKotob.Admin.Core.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public interface IConfigurationValueAccessorFactory
    {
        IConfigurationValueAccessor<T> CreateValueAccessor<T>(string key, T defaultValue, IValidator<T> validator = null);
        IConfigurationConnectionStringAccessor CreateConnectionStringAccessor(string key);
    }
}
