using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.DI
{
    public interface IDependencyContainerWrapper : IDisposable
    {
        object GetWrappedContainer();
        T GetInstance<T>();
        T GetInstance<T>(Dictionary<string, object> args);
    }
}
