using StructureMap;
using StructureMap.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.DI
{
    public class StructureMapContainerWrapper : IDependencyContainerWrapper
    {
        IContainer Container { get;  set; }

        public StructureMapContainerWrapper(IContainer container)
        {
            this.Container = container;
        }


        public void Dispose()
        {
            this.Container?.Dispose();
        }

        public object GetWrappedContainer()
        {
            return Container;
        }

        public T GetInstance<T>()
        {
            var instance = Container.GetInstance<T>();
            return instance;
        }

        public T GetInstance<T>(Dictionary<string, object> args)
        {
            var explicitArgs = new ExplicitArguments() { };
            if (args != null)
                foreach (var item in args)
                    explicitArgs.SetArg(item.Key, item.Value);

            var instance = Container.GetInstance<T>(explicitArgs);
            return instance;
        }
    }
}
