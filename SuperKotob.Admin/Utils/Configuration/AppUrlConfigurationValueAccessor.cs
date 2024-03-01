using SuperKotob.Admin.Utils.Http;
using SuperKotob.Admin.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Configuration
{
    public class AppUrlConfigurationValueAccessor : IConfigurationValueAccessor<string>
    {
        ILogger Logger { get; set; }
        IHttpContextAccessor HttpContextAccessor { get; set; }

        public string Key
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IConfigurationService ConfiugrationService { get; private set; }

        public AppUrlConfigurationValueAccessor(
            IConfigurationService configurationService,
            ILogger logger, 
            IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContextAccessor = httpContextAccessor;
            this.Logger = logger;
            this.ConfiugrationService = configurationService;
        }

        string _AppUrl;
        public async Task<string> GetValueAsync()
        {
            if (string.IsNullOrWhiteSpace(_AppUrl))
            {
                var defaultAppUrl = this.HttpContextAccessor.GetRequestUrl()?.GetLeftPart(UriPartial.Authority);
                _AppUrl = this.ConfiugrationService.GetValue("AppUrl", defaultAppUrl);
            }
            return _AppUrl;
        }
    }
}
