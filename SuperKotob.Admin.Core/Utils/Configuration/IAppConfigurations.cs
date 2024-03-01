using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public interface IAppConfigurations
    {
        IConfigurationValueAccessor<string> AudienceClientSecret { get; }
        IConfigurationValueAccessor<string> AudienceClientId { get; }
        IConfigurationValueAccessor<string> AppUrl { get; }

        IConfigurationValueAccessor<string> TokenEndpointPath { get; }
        IConfigurationValueAccessor<string> TokenIssuer { get; }

        IConfigurationValueAccessor<string> LoggerRollingFilePath { get; }
        IConfigurationConnectionStringAccessor SuperKotobConnectionString { get; }

        IConfigurationValueAccessor<string> ClientProfile { get;  }
        void Init();
        
    }
}
