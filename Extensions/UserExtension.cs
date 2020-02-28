using Blogger.Data;
using Blogger.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blogger.Extensions
{
    public static class UserExtension
    {
        public static string GetToken(this User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppOptionProvider.JwtOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static string GetEncryptedValue(string password)
        {
            string hashValue = System.Convert.ToBase64String(Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivation.Pbkdf2(
                password: password,
                salt: System.Text.Encoding.UTF8.GetBytes(AppOptionProvider.JwtOptions.Secret),
                prf: Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 256 / 8
            ));
            return hashValue;
        }

        public static void SetPassword(this User user, string rawPassword)
        {
            user.Password = GetEncryptedValue(rawPassword);
        }

        public static bool CheckPassword(this User user, string rawPassword)
        {
            return user.Password == GetEncryptedValue(rawPassword);
        }
    }
}
