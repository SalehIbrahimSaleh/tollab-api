using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector.Core
{
    public interface IProtectorOptions
    {
        Task<string> GetAudienceClientIdAsync();
        Task<string> GetAudienceClientSecretAsync();
        Task<string> GetTokenEndpointPathAsync();
        Task<string> GetTokenIssuerAsync();
    }
}
