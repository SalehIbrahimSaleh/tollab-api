using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.DI
{
    public interface IDependencyContainerScope : IDisposable
    {
        IDependencyContainerWrapper GetContainerWapper();
        void DisposeContainerWapper();
        T GetInstance<T>();
        T GetInstance<T>(Dictionary<string, object> args);
    }
}
