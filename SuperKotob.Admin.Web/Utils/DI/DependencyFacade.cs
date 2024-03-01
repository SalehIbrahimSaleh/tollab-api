using SuperKotob.Admin.Utils.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SuperKotob.Admin.Web.Utils.DI
{
    public class DependencyFacade
    {
        public static StructureMapDependencyResolver SetupDefaultResolver(HttpConfiguration httpConfig)
        {
            var resolver = GetDependencyResolver();

            System.Web.Mvc.DependencyResolver.SetResolver(resolver);
            httpConfig.DependencyResolver = resolver;
            //GlobalConfiguration.Configuration.DependencyResolver = resolver;
            return resolver;
        }       

        public static StructureMapDependencyResolver GetDependencyResolver()
        {
            var factory = new AdminDependencyContainerFactory();
            var staticScope = new StaticDependencyContainerScope(factory);            
            var owinScope = new StructureMapOwinContainerScope(factory, staticScope);
            var resolver = new StructureMapDependencyResolver(owinScope);
            
            return resolver;
        }
        
    }
}