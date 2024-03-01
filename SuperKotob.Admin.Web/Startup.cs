using Microsoft.Owin;
using Owin;
using SuperKotob.Admin.Web.Utils.DI;
//using System.Web.Http;
using System.Web.Routing;

[assembly: OwinStartupAttribute(typeof(SuperKotob.Admin.Web.Startup))]
namespace SuperKotob.Admin.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            

        }
    }
}
