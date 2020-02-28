using System;
using Blogger.Data.Dto;

namespace Blogger.Data.Dto
{
    public class PostDto : IRequestDto<Post>
    {
        public Guid Id { set; get; }
        public string Title {set; get;}
        public string Body {set; get;}
        public DateTime CreatedOn {set; get;}

        public ErrorDto Error = ErrorDto.Empty();

        public bool IsValid(DtoTypes.RequestType type)
        {
            bool validInstance = IsValidTitle() && IsValidBody() && IsValidDateTime();

            if (type == DtoTypes.RequestType.Create)
            {
                return validInstance;
            }
            else if(type == DtoTypes.RequestType.Update)
            {
                return validInstance && IsValidId();
            }
            return false;
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

        private bool IsValidId()
        {
            if(Id == Guid.Empty)
            {
                Error.Field = "id";
                Error.Message = "Post Id is required";
                return false;
            }
            return true;
        }

        private bool IsValidDateTime()
        {
            if(CreatedOn <= DateTime.MinValue)
            {
                Error.Field = "createdOn";
                Error.Message = "required a valid Date-Time";
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