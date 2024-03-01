using SuperKotob.Admin.Utils.Configuration;
using SuperMatjar.WebProtector.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector
{
    public class ConfigurableProtectorOptions : IProtectorOptions
    {
        IAppConfigurations AppConfigurations { get; set; }

        public ConfigurableProtectorOptions(IAppConfigurations appConfigurations)
        {
            this.AppConfigurations = appConfigurations;
        }


        public async Task<string> GetAudienceClientIdAsync()
        {
            return await AppConfigurations.AudienceClientId.GetValueAsync();
        }

        public async Task<string> GetAudienceClientSecretAsync()
        {
            return await AppConfigurations.AudienceClientSecret.GetValueAsync();
        }

        public async Task<string> GetTokenEndpointPathAsync()
        {
            return await AppConfigurations.TokenEndpointPath.GetValueAsync();
        }

        public async Task<string> GetTokenIssuerAsync()
        {
            return await AppConfigurations.TokenIssuer.GetValueAsync();
        }
    }
}
