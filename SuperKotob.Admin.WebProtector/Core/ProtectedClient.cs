using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector.Core
{
    public class ProtectedClient
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public bool IsConfidential { get; set; }
        public bool IsActive { get; set; }
        public int RefreshTokenLifetime { get; set; }
        public string AllowedOrigin { get; set; }
    }
}
