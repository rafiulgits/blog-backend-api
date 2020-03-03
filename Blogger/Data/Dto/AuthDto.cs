using System;

namespace Blogger.Data.Dto
{
    public class AuthDto : IDto<Object>
    {
        public string Email { set; get; }
        public string Password { set; get; }

        public Object GetPersistentObject()
        {
            return null;
        }
    }
}
