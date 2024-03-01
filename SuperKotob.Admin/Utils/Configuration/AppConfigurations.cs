using SuperKotob.Admin.Core.Utils.Validation;
using SuperKotob.Admin.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public class AppConfigurations : IAppConfigurations
    {
        public IConfigurationValueAccessor<string> AudienceClientSecret { get; private set; }

        public IConfigurationValueAccessor<string> AudienceClientId { get; private set; }

        public IConfigurationValueAccessor<string> AppUrl { get; private set; }

        public IConfigurationValueAccessor<string> TokenEndpointPath { get; private set; }

        public IConfigurationValueAccessor<string> TokenIssuer { get; private set; }
        public IConfigurationValueAccessor<string> LoggerRollingFilePath { get; private set; }
        public IConfigurationValueAccessor<string> ClientProfile { get; private set; }
        public IConfigurationValueAccessorFactory AccessorFactory { get; private set; }

        public IConfigurationConnectionStringAccessor SuperKotobConnectionString { get; private set; }

        public AppConfigurations(IConfigurationValueAccessorFactory accessorFactory)
        {
            this.AccessorFactory = accessorFactory;
            this.Init();
        }

        public void Init()
        {
            var notEmptyValidator = new NotEmptyTextValidator();
            var compoValidator = new CompositeTextValidator(
                notEmptyValidator,
                new UrlTextValidator()
                );

            AudienceClientSecret = AccessorFactory.CreateValueAccessor("AudienceClientSecret", "", notEmptyValidator);
            AudienceClientId = AccessorFactory.CreateValueAccessor("AudienceClientId", "", notEmptyValidator);
            AppUrl = AccessorFactory.CreateValueAccessor("AppUrl", "", compoValidator);
            TokenEndpointPath = AccessorFactory.CreateValueAccessor("TokenEndpointPath", "");
            TokenIssuer = AccessorFactory.CreateValueAccessor("TokenIssuer", "");
            LoggerRollingFilePath = AccessorFactory.CreateValueAccessor("LoggerRollingFilePath", "logs\\SuperKotob-{Date}.txt");
            SuperKotobConnectionString = AccessorFactory.CreateConnectionStringAccessor("SuperKotobContext");
            ClientProfile = AccessorFactory.CreateValueAccessor("ClientProfile", "", null);
        }
    }
}


