using Blogger.Data;
using Blogger.Util;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public interface IUserService
    {
        Task<DataDependentResult<User>> Create(User user);
        Task<User> Get(int id);
    }
}
