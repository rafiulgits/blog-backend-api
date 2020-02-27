using System;
using System.ComponentModel.DataAnnotations;

namespace Blogger.Data
{
    public class Post :IEntity<Guid>
    {
        public Guid Id {set; get;}

        [Required]
        [StringLength(250, MinimumLength=1)]
        public string Title {set; get;}

        [Required]
        public string Body {set; get;}

        [Required]
        public DateTime CreatedOn {set; get;}
        
        public DateTime LastUpdateOn {set; get;}
    }
}