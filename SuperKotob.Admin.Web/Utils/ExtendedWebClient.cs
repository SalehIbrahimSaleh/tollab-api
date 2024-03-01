
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Tollab.Admin.Web.Utils
{
    public class ExtendedWebClient : WebClient
    {
        public int Timeout { get; set; }
        public new bool AllowWriteStreamBuffering { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = Timeout;
                var httpRequest = request as HttpWebRequest;
                if (httpRequest != null)
                {
                    httpRequest.AllowWriteStreamBuffering = AllowWriteStreamBuffering;
                }
            }
            return request;
        }

        public ExtendedWebClient()
        {
            Timeout = 100000; // the standard HTTP Request Timeout default
        }
    }
}