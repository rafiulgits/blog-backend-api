using System;

namespace Blogger.Data.Dto
{
    public class PostDto : IDto<Post>
    {
        public Guid Id { set; get; } = Guid.Empty;
        public string Title { set; get; } = string.Empty;
        public string Body { set; get; } = string.Empty;
        public DateTime CreatedOn { set; get; }
        public int AuthorId { set; get; }
        public AuthorDto Author { set; get; }
       

        public PostDto()
        {

        }

        public PostDto(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            Body = post.Body;
            CreatedOn = post.CreatedOn;
            AuthorId = post.AuthorId;
            Author = new AuthorDto(post.Author);
        }


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