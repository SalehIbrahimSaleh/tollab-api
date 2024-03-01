using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.UseCases.PushTokens
{
    public class PushTokenPushDTO
    {
        public long UserId { get; set; }
        public long? UserTypeId { get; set; }
        public string OS { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
