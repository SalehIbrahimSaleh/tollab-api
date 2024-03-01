using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core.Utils.Validation
{
    public interface IValidator<T>
    {
        Task<ValidatorResult> ValidateAsync(ValidatorContext<T> context);
    }
}
