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
    public class UserService
    {
        public UserRepository UserRepo;

        public UserService(UserRepository userRepository)
        {
            UserRepo = userRepository;
        }

        public async Task<User> Create(User user)
        {
            return await UserRepo.Add(user);
        }

        public async Task<User> Get(string id)
        {
            int _id = int.Parse(id);
            return await UserRepo.Get(_id);
        }
    }
}
