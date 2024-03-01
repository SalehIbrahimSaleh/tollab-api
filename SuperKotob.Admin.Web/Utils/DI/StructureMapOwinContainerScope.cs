using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SuperKotob.Admin.Utils.DI;

namespace SuperKotob.Admin.Web.Utils.DI
{
    public class StructureMapOwinContainerScope : IDependencyContainerScope
    {
        private const string NestedContainerKey = "Nested.Container.Key";

        public StaticDependencyContainerScope ParentScope { get; private set; }
        public IDependencyContainerFactory ContainerFactory { get; private set; }

        public StructureMapOwinContainerScope(
            IDependencyContainerFactory containerFactory, 
            StaticDependencyContainerScope parentScope)
        {
            this.ContainerFactory = containerFactory;
            this.ParentScope = parentScope;
        }

        public void DisposeContainerWapper()
        {
            var context = HttpContext.Current?.Request?.GetOwinContext();
            if (context == null)
                return;

            var containerWrapper = context.Get<IDependencyContainerWrapper>(NestedContainerKey);
            containerWrapper.Dispose();
        }

        public void Dispose()
        {
            DisposeContainerWapper();
        }

        public T GetInstance<T>()
        {
            return GetContainerWapper().GetInstance<T>();
        }
        public T GetInstance<T>(Dictionary<string, object> args)
        {
            return GetContainerWapper().GetInstance<T>(args);
        }
        public IDependencyContainerWrapper GetContainerWapper()
        {
            var isOwinContextAvailable = false;
            try
            {
                isOwinContextAvailable = HttpContext.Current.GetOwinContext() != null;
            }
            catch (Exception ex)
            {
            }

            var containerWrapper = isOwinContextAvailable
                ? GetContainerFromOwinContext()
                : GetContainerFromParentScope();

            return containerWrapper;
        }
        IDependencyContainerWrapper GetContainerFromParentScope()
        {
            var containerWrapper = ParentScope == null
                ? null
                : ParentScope.GetContainerWapper();

            return containerWrapper;
        }
        IDependencyContainerWrapper GetContainerFromOwinContext()
        {
            var context = HttpContext.Current.GetOwinContext();
            var containerWrapper = context.Get<IDependencyContainerWrapper>(NestedContainerKey);

            if (containerWrapper == null)
            {
                var parentWrapper = ParentScope.GetContainerWapper();
                containerWrapper = ContainerFactory.CreateNestedContainer(parentWrapper);
                context.Set(NestedContainerKey, containerWrapper);
            }
            return containerWrapper;
        }

    }
}