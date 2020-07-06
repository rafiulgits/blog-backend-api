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
        public string FirstName { set; get; } = string.Empty;
        public string LastName { set; get; } = string.Empty;
        public string Email { set; get; } = string.Empty;
        public string Password { set; get; } = string.Empty;
        public string BlogName { set; get; } = string.Empty;

        public User GetPersistentObject()
        {
            return new User()
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email.ToLower(),
                BlogName = this.BlogName.ToLower(),
                Password = this.Password
            };
        }

    }
}
