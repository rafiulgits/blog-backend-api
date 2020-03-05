using Blogger.Data;
using Blogger.Extensions;
using Blogger.Util;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public class UserService : DataDependentService<User>, IUserService
    {
        public IUserRepository UserRepo;

        public UserService(IUserRepository userRepository)
        {
            UserRepo = userRepository;
        }

        public async Task<DataDependentResult<User>> Create(User user)
        {
            User exists = null;
            Result.IsValid = true;
            exists = UserRepo.GetQueryableHandler().Where(item => item.Email == user.Email).FirstOrDefault();
            if(exists != null)
            {
                Result.IsValid = false;
                Result.Error.Append("Email", "a user already exists with this email address");
            }
            exists = UserRepo.GetQueryableHandler().Where(item => item.BlogName == user.BlogName).FirstOrDefault();
            if(exists != null)
            {
                Result.IsValid = false;
                Result.Error.Append("BlogName", "a user already exists with this blog name");
            }
            if(Result.IsValid)
            {
                user.SetPassword(user.Password);
                Result.Data = await UserRepo.Add(user);
            }
            return Result;
        }

        public async Task<User> Get(string id)
        {
            int _id = int.Parse(id);
            return await UserRepo.Get(_id);
        }
    }
}
