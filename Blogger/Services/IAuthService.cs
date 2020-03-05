using Blogger.Data;
using Blogger.Util;


namespace Blogger.Services
{
    public interface IAuthService
    {
        DataDependentResult<User> Authenticate(string email, string password);
    }
}
