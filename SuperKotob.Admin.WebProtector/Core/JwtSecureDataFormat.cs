using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Tokens;

namespace SuperMatjar.WebProtector.Core
{
    public class JwtSecureDataFormat : ISecureDataFormat<AuthenticationTicket>
    {

        IProtectorOptions ProtectorOptions { get; set; }
        public JwtSecureDataFormat(IProtectorOptions authOptions)
        {
            this.ProtectorOptions = authOptions;
        }


        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var audienceId = Task.Run(ProtectorOptions.GetAudienceClientIdAsync).Result;
            var signingKey = Task.Run(ProtectorOptions.GetAudienceClientSecretAsync).Result;
            var signingKeyArray = TextEncodings.Base64Url.Decode(signingKey);
            var signingCredentials = new HmacSigningCredentials(signingKeyArray);
            var issuer = Task.Run(ProtectorOptions.GetTokenIssuerAsync).Result;
            var claims = data.Identity.Claims;
            var issuedAt = data.Properties.IssuedUtc.Value.UtcDateTime;
            var expiresAt = data.Properties.ExpiresUtc.Value.UtcDateTime;

            var token = new JwtSecurityToken(
                issuer,
                audienceId, 
                claims, 
                issuedAt, 
                expiresAt, 
                signingCredentials);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            return jwt;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}