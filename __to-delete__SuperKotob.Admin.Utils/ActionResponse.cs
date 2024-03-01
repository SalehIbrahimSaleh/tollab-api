using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Utils
{
    public class ActionResponse<T>
    {
        public T Value { get; set; }
        public SuperKotobError Error { get; set; }
    }
}
