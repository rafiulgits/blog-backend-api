using Blogger.Data;
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

        public User Authenticate(string email, string hashedPassword)
        {
            var resource = UserRepo.GetQueryableHandler();
            var result = resource.Where(user => 
                user.Email.IndexOf(email, StringComparison.OrdinalIgnoreCase) != -1 && 
                user.Password == hashedPassword).FirstOrDefault();
            return result;
        }
    }
}
