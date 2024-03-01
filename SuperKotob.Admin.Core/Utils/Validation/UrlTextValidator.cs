using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core.Utils.Validation
{
    public class UrlTextValidator : IValidator<string>
    {
        public async Task<ValidatorResult> ValidateAsync(ValidatorContext<string> context)
        {
            Uri uri;
            var isValid = Uri.TryCreate(context.Value, UriKind.RelativeOrAbsolute, out uri);

            var list = isValid ? null : new List<string>(){
                     $"{context.Key} is not a valid url"
                };

            return new ValidatorResult()
            {
                Messages = list
            };
        }
    }
}
