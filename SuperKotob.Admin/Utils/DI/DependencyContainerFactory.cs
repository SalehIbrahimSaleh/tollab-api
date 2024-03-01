using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.DI
{
    public class DependencyContainerFactory : IDependencyContainerFactory
    {
        public virtual IDependencyContainerWrapper CreateConfiguredContainer()
        {
            var container = new Container(new AppRegistry());
            return new StructureMapContainerWrapper(container);
        }

        public virtual IDependencyContainerWrapper CreateContainer()
        {
            var container = new Container();
            return new StructureMapContainerWrapper(container);
        }

        public virtual IDependencyContainerWrapper CreateNestedContainer(IDependencyContainerWrapper parentContainer)
        {
            var container = parentContainer.GetWrappedContainer() as IContainer;
            var nestedContainer = container.GetNestedContainer();
            return new StructureMapContainerWrapper(nestedContainer);
        }
    }
}
