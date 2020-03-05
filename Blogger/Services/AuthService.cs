using Blogger.Data;
using Blogger.Extensions;
using Blogger.Util;
using System.Linq;


namespace Blogger.Services
{
    public class AuthService : DataDependentService<User>, IAuthService
    {
        private readonly IUserRepository UserRepo;
        public AuthService(IUserRepository userRepository)
        {
            UserRepo = userRepository;
        }

        public DataDependentResult<User> Authenticate(string email , string password)
        {
            User user = UserRepo.GetQueryableHandler()
                                .Where(user => user.Email == email)
                                .FirstOrDefault();
            if(user != null)
            {
                if (user.CheckPassword(password))
                {
                    Result.Data = user;
                    Result.IsValid = true;
                }
                else
                {
                    Result.Error.Append("Password", "incorrect password");
                }
            }
            else
            {
                Result.Error.Append("Email", "no user found with this email address");
            }
            return Result;
        }
    }
}
