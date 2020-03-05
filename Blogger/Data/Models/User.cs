using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blogger.Data
{
    public class User : IEntity<int>
    {
        public int Id { set; get; }
        
        [Required]
        [StringLength(128, MinimumLength=1)]
        public string FirstName { set; get; }

        [Required]
        [StringLength(128, MinimumLength=1)]
        public string LastName { set; get; }

        [Required]
        [StringLength(500, MinimumLength=10)]
        [EmailAddress(ErrorMessage="invalid email address")]
        public string Email { set; get; }

        [Required]
        [MinLength(8)]
        public string Password { set; get; }

        [Required]
        [StringLength(30, MinimumLength=3)]
        public string BlogName { set; get; }

        List<Post> Blog { set; get; }
    }
}
