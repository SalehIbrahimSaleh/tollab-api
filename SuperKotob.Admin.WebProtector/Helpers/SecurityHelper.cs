using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SuperMatjar.WebProtector.Helpers
{
    public static class SecurityHelper
    {
        public static string ComputeHash(string text)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();
            byte[] byteValue = Encoding.UTF8.GetBytes(text);
            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);
            return hash;
        }
    }
}
