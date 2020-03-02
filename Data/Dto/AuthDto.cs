using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data.Dto
{
    public class AuthDto : IRequestDto<User>
    {
        [EmailAddress]
        public string Email { set; get; }
        public string Password { set; get; }

        public ErrorDto Error = ErrorDto.Empty();

        public bool IsValid(DtoTypes.RequestType type)
        {
            return true;
        }

        public User GetPersistentObject()
        {
            return null;
        }
    }
}
