using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Extensions
{
    public static class HttpContextExtension
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            if(String.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                return -1;
            }
            return int.Parse(httpContext.User.Identity.Name);
        }
    }
}
