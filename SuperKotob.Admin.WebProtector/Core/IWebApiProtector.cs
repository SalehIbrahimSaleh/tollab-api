using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperMatjar.WebProtector.Core
{
    public interface IWebApiProtector
    {
        void Configure(IAppBuilder app);
    }
}