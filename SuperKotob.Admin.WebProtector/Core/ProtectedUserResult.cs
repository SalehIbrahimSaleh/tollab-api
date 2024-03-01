using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector.Core
{
    public class ProtectedUserResult
    {
        public ProtectedUserResult(bool succeeded)
            : this(succeeded, null)
        {
        }

        public ProtectedUserResult(bool succeeded, IList<string> errors)
        {
            this.Succeeded = succeeded;
            this.Errors = errors;
        }
        public IList<string> Errors { get; private set; }
       
        public bool Succeeded { get; private set; }
    }
}
