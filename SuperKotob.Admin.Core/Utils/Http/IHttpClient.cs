using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostJsonAsync(string url, dynamic data);
    }
}
