using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Http
{
    public interface IHttpContextAccessor
    {
        Uri GetRequestUrl();
    }
}
