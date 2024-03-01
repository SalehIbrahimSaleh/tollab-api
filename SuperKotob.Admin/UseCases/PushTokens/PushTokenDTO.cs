using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperKotob.UseCases.PushTokens
{
    public class PushTokenDTO
    {
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public long? CustomerTypeId { get; set; }
        public string Token { get; set; }
        public string OS { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }


    }
}
