using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core.Utils.Validation
{
    public class ValidatorContext<T>
    {
        public ValidatorContext(string key, T value)
        {
            this.Key = key;
            this.Value = value;
        }

        public T Value { get; set; }
        public RequestInputs ContextInputs { get; set; }
        public string Key { get; private set; }
    }
}
