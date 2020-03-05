using System;
using Blogger.Data.Dto;

namespace Blogger.Data.Dto
{
    public class PostDto : IDto<Post>
    {
        public Guid Id { set; get; }
        public string Title {set; get;}
        public string Body {set; get;}
        public DateTime CreatedOn {set; get;}
        
        public int AuthorId;

        public Post GetPersistentObject()
        {
            return new Post()
            {
                Id = this.Id,
                Title = this.Title,
                Body = this.Body,
                CreatedOn = this.CreatedOn,
                AuthorId = this.AuthorId
            };
        }
    }
}