using System;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SuperMatjar.WebProtector.Core;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector
{
    public class OAuthHttpApiProtector : IWebApiProtector
    {
        IProtectorOptions AuthOptions { get; set; }
        IOAuthAuthorizationServerProvider OAuthAuthorizationServerProvider { get; set; }
        IAuthenticationTokenProvider AuthenticationTokenProvider { get; set; }
        ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; set; }

        public OAuthHttpApiProtector(
            IAuthenticationTokenProvider authenticationTokenProvider,
            IOAuthAuthorizationServerProvider oAuthAuthorizationServerProvider,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat,
            IProtectorOptions authOptions)
        {
            this.AuthenticationTokenProvider = authenticationTokenProvider;
            this.OAuthAuthorizationServerProvider = oAuthAuthorizationServerProvider;
            this.AccessTokenFormat = accessTokenFormat;
            this.AuthOptions = authOptions;
        }


        public void Configure(IAppBuilder app)
        {
            ConfigureTokenGeneration(app);
            ConfigureTokenConcumption(app);
            ConfigureDatabase(app);
        }

        private void ConfigureDatabase(IAppBuilder app)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProtectorDbContext, WebProtector.Migrations.Configuration>());
        }

        private void ConfigureTokenGeneration(IAppBuilder app)
        {
            var provider = OAuthAuthorizationServerProvider;
            var refreshTokenProvider = AuthenticationTokenProvider;
            var accessTokenFormat = AccessTokenFormat;
            var tokenEndpointPath = Task.Run(AuthOptions.GetTokenEndpointPathAsync).Result;

            var oAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString(tokenEndpointPath),
                Provider = provider,
                RefreshTokenProvider = refreshTokenProvider,
                AccessTokenFormat = accessTokenFormat
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
        }

        private void ConfigureTokenConcumption(IAppBuilder app)
        {
            var issuer = Task.Run(AuthOptions.GetTokenIssuerAsync).Result;
            var audienceId = Task.Run(AuthOptions.GetAudienceClientIdAsync).Result;
            var audienceSecret = Task.Run(AuthOptions.GetAudienceClientSecretAsync).Result;
            var audienceSecretArray = TextEncodings.Base64Url.Decode(audienceSecret);

            var tokenProviders = new IIssuerSecurityTokenProvider[]
            {
                new SymmetricKeyIssuerSecurityTokenProvider(issuer,audienceSecretArray)
            };

            var options = new JwtBearerAuthenticationOptions()
            {
                AuthenticationMode = AuthenticationMode.Active,
                AllowedAudiences = new[] { audienceId },
                IssuerSecurityTokenProviders = tokenProviders
            };

            app.UseJwtBearerAuthentication(options);
        }

    }
}