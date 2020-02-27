using System;

namespace Blogger.Data.Dto
{
    public class ErrorDto
    {
        public string Field { set; get; } = String.Empty;
        public string Message { set; get; } = String.Empty;

        public static ErrorDto Empty()
        {
            return new ErrorDto();
        }
    }
}
