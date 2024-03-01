using Microsoft.Practices.ServiceLocation;
using SuperKotob.Admin.Utils.DI;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;

namespace SuperKotob.Admin.Web.Utils.DI
{
    public class StructureMapDependencyResolver : ServiceLocatorImplBase, IDependencyResolver
    {
        public IDependencyContainerScope ContainerScope { get; private set; }

        public StructureMapDependencyResolver(IDependencyContainerScope containerScope)
        {
            this.ContainerScope = containerScope;
        }

        IContainer GetContainer()
        {
            var _container = ContainerScope.GetContainerWapper()?.GetWrappedContainer() as IContainer;
            return _container;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return DoGetAllInstances(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return GetContainer().GetAllInstances(serviceType).Cast<object>();
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            object instance;
            if (string.IsNullOrEmpty(key))
            {
                instance = serviceType.IsAbstract || serviceType.IsInterface
                    ? GetContainer().TryGetInstance(serviceType)
                    : GetContainer().GetInstance(serviceType);
            }
            else
            {

                instance = GetContainer().GetInstance(serviceType, key);
            }
            return instance;
        }

        public IDependencyScope BeginScope()
        {
            var newScope = DependencyFacade.GetDependencyResolver();
            return newScope;
        }

        public void Dispose()
        {
            ContainerScope.DisposeContainerWapper();
        }

    }
}