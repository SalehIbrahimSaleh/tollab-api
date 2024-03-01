using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core.Utils.Validation
{
    public class NotEmptyTextValidator : IValidator<string>
    {
        public async Task<ValidatorResult> ValidateAsync(ValidatorContext<string> context)
        {
            var isValid = !string.IsNullOrWhiteSpace(context.Value);
            var list = isValid ? null : new List<string>(){
                     $"{context.Key} can not be empty"
                };

            return new ValidatorResult()
            {
                Messages = list
            };
        }
    }
}
