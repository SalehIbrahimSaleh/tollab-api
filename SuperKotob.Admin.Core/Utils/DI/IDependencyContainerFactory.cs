using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.DI
{
    public interface IDependencyContainerFactory
    {
        IDependencyContainerWrapper CreateContainer();
        IDependencyContainerWrapper CreateConfiguredContainer();
        IDependencyContainerWrapper CreateNestedContainer(IDependencyContainerWrapper wapper);
    }
}
