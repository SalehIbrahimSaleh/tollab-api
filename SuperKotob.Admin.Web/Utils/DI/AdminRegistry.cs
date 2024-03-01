using SuperKotob.Admin.Utils.DI;
using SuperKotob.Admin.Utils.Logging;
using StructureMap;
using SuperKotob.Admin.Utils.Http;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using SuperKotob.Admin.Web.Models;
using System.Web;

namespace SuperKotob.Admin.Web.Utils.DI
{
    public class AdminRegistry : Registry
    {
        public AdminRegistry()
        {
            IncludeRegistry<AppRegistry>();
            //For<IExceptionLogger>().Use<RestApiLogger>();
            //For<ILogger>().Use<RestApiLogger>();

            For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);

            For<IUserStore<ApplicationUser>>()
            .Use<UserStore<ApplicationUser>>()
            .Ctor<DbContext>()
            .Is<ApplicationDbContext>(cfg => cfg.SelectConstructor(() => new ApplicationDbContext()));

            ForConcreteType<UserManager<ApplicationUser>>()
                .Configure
                .SetProperty(userManager => userManager.PasswordValidator = new PasswordValidator
                {
                    RequiredLength = 6
                })
                .SetProperty(userManager => userManager.UserValidator = 
                new UserValidator<ApplicationUser>(userManager));

            For<IDependencyContainerFactory>().Use<AdminDependencyContainerFactory>();
            For<IDependencyContainerScope>().Use<StructureMapOwinContainerScope>();

            //For<IWebApiProtector>().Use<OAuthHttpApiProtector>();
            //For<IHttpContextAccessor>().Use<HttpContextAccessor>();

            // For<IAuthenticationTokenProvider>().Use<RefreshTokenProvider>();
            //For<IOAuthAuthorizationServerProvider>().Use<ProtectedAuthorizationServerProvider>();
            // For<ISecureDataFormat<AuthenticationTicket>>().Use<JwtSecureDataFormat>();
            //For<IProtectorOptions>().Use<ConfigurableProtectorOptions>();
            //For<IProtectedUserRepository>().Use<AspnetIdentityUserRepository>();

        }
    }
}