using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required]
        [ForeignKey("Author")]
        public int AuthorId { set; get; }

        public User Author { set; get; }
    }
}