using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Blogger.Data.Dto
{
    public class ErrorDto : ModelStateDictionary
    {
        public ErrorDto()
        {

        }

        public ErrorDto Append(string field, string message)
        {
            this.AddModelError(field, message);
            return this;
        }
    }
}
