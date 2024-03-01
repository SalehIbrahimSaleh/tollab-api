using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.Admin.Data.TransferObjects
{
    public class SignUpConfirmResponseDTO
    {
        public long CustomerId { get; internal set; }
        public string Password { get; internal set; }
        public string Name { get; internal set; }
        public string Phone { get; internal set; }
        public string Email { get; internal set; }
    }
}
