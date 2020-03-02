using Blogger.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Util
{
    public class Crypto
    {
        public static string GetHashedPassword(string password)
        {
            string hashValue = Convert.ToBase64String(Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivation.Pbkdf2(
               password: password,
               salt: System.Text.Encoding.UTF8.GetBytes(AppOptionProvider.JwtOptions.Secret),
               prf: Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf.HMACSHA256,
               iterationCount: 1000,
               numBytesRequested: 256 / 8
           )); ;
            return hashValue;
        }
    }
}
