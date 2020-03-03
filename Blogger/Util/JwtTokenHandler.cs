using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Util
{
    public class JwtTokenHandler
    {
        private static JwtSecurityTokenHandler tokenHandler;

        public static JwtSecurityTokenHandler GetTokenHandler()
        {
            if(tokenHandler == null)
            {
                tokenHandler = new JwtSecurityTokenHandler();
            }
            return tokenHandler;
        }
    }
}
