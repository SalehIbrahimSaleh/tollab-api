using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Data.TransferObjects
{
    public class SignUpConfirmDTO
    {
        public string ConfirmationCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
