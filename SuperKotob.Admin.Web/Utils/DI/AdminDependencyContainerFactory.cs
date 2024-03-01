using StructureMap;
using SuperKotob.Admin.Utils.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperKotob.Admin.Web.Utils.DI
{
    public class AdminDependencyContainerFactory : DependencyContainerFactory
    {

        public override IDependencyContainerWrapper CreateConfiguredContainer()
        {
            var registry = new AdminRegistry();
            //registry.IncludeRegistry<RestApiRegistry>();
            //registry.IncludeRegistry<UtilsRegistry>();

            var container = new Container(registry);
            return new StructureMapContainerWrapper(container);
        }
    }
}