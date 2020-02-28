using System;

namespace Blogger.Data.Dto
{
    public class PostDto : IRequestDto<Post>
    {
        public Guid Id { set; get; }
        public string Title {set; get;}
        public string Body {set; get;}
        public DateTime CreatedOn {set; get;}

        public ErrorDto Error = ErrorDto.Empty();

        public bool IsValid()
        {
            return IsValidTitle() && IsValidBody();
        }

        private bool IsValidTitle()
        {
            if(String.IsNullOrEmpty(Title))
            {
                Error.Field = "title";
                Error.Message = "title shouldn't be null or empty";
                return false;
            }
                
            if(Title.Length < 1 || Title.Length > 250)
            {
                Error.Field = "title";
                Error.Message = "title length should between 1 to 255";
                return false;
            }
            return true;
        }

        private bool IsValidBody()
        {
            if(String.IsNullOrWhiteSpace(Body))
            {
                Error.Field = "body";
                Error.Message = "only whitespace is not allowed in body";
                return false;
            }
            return true;
        }

        public Post GetPersistentObject()
        {
            return new Post()
            {
                Title = this.Title,
                Body = this.Body,
                CreatedOn = this.CreatedOn
            };
        }
    }
}