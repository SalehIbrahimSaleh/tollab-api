using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core.Utils.Validation
{
    public class CompositeTextValidator : IValidator<string>
    {
        IValidator<string>[] _validators;
        public CompositeTextValidator(params IValidator<string>[] validators)
        {
            _validators = validators;
        }
        public async Task<ValidatorResult> ValidateAsync(ValidatorContext<string> context)
        {
            if (_validators == null)
                return new ValidatorResult
                {
                    Messages = new List<string>(){
                        "No validators passed to the composite text validator"
                    }
                };

            foreach(var validator in _validators)
            {
                var result = await validator.ValidateAsync(context);
                if (!result.IsValid)
                    return result;
            }

            return new ValidatorResult(); ;
        }
    }
}
