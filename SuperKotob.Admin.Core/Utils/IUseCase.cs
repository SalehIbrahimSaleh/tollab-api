using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils
{
    public interface IUseCase<IRequestMessage, IResponseMessage>
    {
        Task<IResponseMessage> RunAsync(IRequestMessage requestMessage);
    }
}
