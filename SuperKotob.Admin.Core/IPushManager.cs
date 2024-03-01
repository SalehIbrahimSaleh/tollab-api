using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core
{
    public interface IPushManager
    {
        void Push(string token, string message, string os, string title, string url);
    }
}
