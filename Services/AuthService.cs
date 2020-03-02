using Blogger.Data;
using Blogger.Data.Dto;
using Blogger.Extensions;
using Blogger.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public class AuthService
    {
        private readonly UserRepository UserRepo;
        public AuthService(UserRepository userRepository)
        {
            UserRepo = userRepository;
        }

        public User Authenticate(AuthDto authDto)
        {
            string requestEmail = authDto.Email.ToLower();
            string requestPassword = authDto.Password;

            User user = UserRepo.GetQueryableHandler().Where(user => user.Email == requestEmail)
                                                      .FirstOrDefault();
            if(user != null)
            {
                if (user.CheckPassword(requestPassword))
                {
                    return user;
                }
            }
            return null;
        }
    }
}
