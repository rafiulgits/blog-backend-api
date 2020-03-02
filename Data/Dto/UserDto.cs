using Blogger.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data.Dto
{
    public class UserDto : IDto<User>
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Email { set; get; }
        public string Password { set; get; }
        public string BlogName { set; get; }

        public User GetPersistentObject()
        {
            User user = new User()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email.ToLower(),
                BlogName = this.BlogName.ToLower(),
                Password = this.Password
            };
            return user;
        }

    }
}
