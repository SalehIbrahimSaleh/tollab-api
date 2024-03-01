using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace SuperKotob.Admin.Utils.DI
{
    public class StaticDependencyContainerScope : IDependencyContainerScope
    {
        object syncObject = new object();
        static IDependencyContainerWrapper DependencyContainer { get; set; }
        IDependencyContainerFactory DependencyContainerFactory { get; set; }

        public StaticDependencyContainerScope(IDependencyContainerFactory containerFactory)
        {
            this.DependencyContainerFactory = containerFactory;
        }


        public bool CanGet()
        {
            return true;
        }

        public void Dispose()
        {
        }

        public void DisposeContainerWapper()
        {
        }

        public IDependencyContainerWrapper GetContainerWapper()
        {
            if (DependencyContainer == null)
            {
                lock (syncObject)
                {
                    if (DependencyContainer == null)
                        DependencyContainer = DependencyContainerFactory.CreateConfiguredContainer();
                }
            }
            return DependencyContainer;
        }

        public T GetInstance<T>()
        {
            return GetContainerWapper().GetInstance<T>();
        }
        public T GetInstance<T>(Dictionary<string, object> args)
        {
            return GetContainerWapper().GetInstance<T>(args);
        }
    }
}
