using Blogger.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data.Dto
{
    public class UserDto : IRequestDto<User>
    {

        [EmailAddress]
        public string Email { set; get; }
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string BlogName { set; get; }

        public ErrorDto Error = ErrorDto.Empty();

        public bool IsValid(DtoTypes.RequestType type)
        {
            return true;
        }

        public User GetPersistentObject()
        {
            User user = new User()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email.ToLower(),
                BlogName = BlogName.ToLower(),
            };
            user.SetPassword(Password);
            return user;
        }

    }
}
