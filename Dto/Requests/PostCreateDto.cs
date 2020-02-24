using System;
using Blogger.Data;

namespace Blogger.Dto
{
    public class PostCreateDto : IRequestDto<Post>
    {

        public string Title {set; get;}
        public string Body {set; get;}
        public DateTime CreatedOn {set; get;}

        public bool IsValid()
        {
            if(IsValidTitle() && IsValidBody())
                return true;
            return false;
        }

        private bool IsValidTitle()
        {
            if(String.IsNullOrEmpty(Title))
                return false;
            if(Title.Length < 1 || Title.Length > 250)
                return false;
            return true;
        }

        private bool IsValidBody()
        {
            if(String.IsNullOrEmpty(Body))
                return false;
            if(String.IsNullOrWhiteSpace(Body))
                return false;
            return true;
        }

        public Post GetObject()
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