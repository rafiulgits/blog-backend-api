using System;

namespace Blogger.Data.Dto
{
    public class AuthDto : IDto<Object>
    {
        public string Email { set; get; } = string.Empty;
        public string Password { set; get; } = string.Empty;

        public Object GetPersistentObject()
        {
            return null;
        }
    }
}
