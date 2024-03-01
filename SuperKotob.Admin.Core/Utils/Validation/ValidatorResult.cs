using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Core.Utils.Validation
{
    public class ValidatorResult
    {
        public ValidatorResult()
        {
            Messages = new List<string>();
        }
        public bool IsValid
        {
            get
            {
                return Messages == null || Messages.Count == 0;
            }
        }
        public IList<string> Messages { get; set; }
        public ValidatorResult AddMessage(string message)
        {
            if (Messages == null)
                Messages = new List<string>();

            Messages.Add(message);
            return this;
        }
    }
}
