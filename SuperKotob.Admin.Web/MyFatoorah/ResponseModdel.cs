using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tollab.Admin.Web.MyFatoorah
{
    public class ResponseModdel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object ValidationErrors { get; set; }
        public Data Data { get; set; }
    }
}