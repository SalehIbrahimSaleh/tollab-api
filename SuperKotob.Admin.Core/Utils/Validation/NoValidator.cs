using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core.Utils.Validation
{
    public class NoValidator<T> : IValidator<T>
    {
        public async Task<ValidatorResult> ValidateAsync(ValidatorContext<T> value)
        {
            return new ValidatorResult();
        }
    }
}
