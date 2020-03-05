using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;


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

        public static string GetCurrentRequestUrl(this HttpContext httpContext)
        {

            return $"{httpContext.Request.GetDisplayUrl()}";
        }
    }
}
