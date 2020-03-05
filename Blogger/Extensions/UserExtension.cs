using Blogger.Data;
using Blogger.Options;
using Blogger.Util;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

namespace Blogger.Extensions
{
    public static class UserExtension
    {
        public static string GetToken(this User user)
        {
            var tokenHandler = JwtTokenHandler.GetTokenHandler();
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


        public static void SetPassword(this User user, string rawPassword)
        {
            user.Password = Crypto.GetHashedPassword(rawPassword);
        }

        public static bool CheckPassword(this User user, string rawPassword)
        {
            return user.Password == Crypto.GetHashedPassword(rawPassword);
        }
    }
}
