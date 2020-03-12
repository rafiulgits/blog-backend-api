using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Data.Dto
{
    public class AuthorDto
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Email { set; get; }
        public string BlogName { set; get; }

        public AuthorDto()
        {

        }

        public AuthorDto(User author)
        {
            Id = author.Id;
            Name = $"{author.FirstName} {author.LastName}";
            Email = author.Email;
            BlogName = author.BlogName;
        }
    }
}
