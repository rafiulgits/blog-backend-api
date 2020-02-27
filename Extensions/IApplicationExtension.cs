using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Blogger.Extensions
{
    public static class IApplicationExtension
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>();
                var result = JsonSerializer.Serialize(new {error = exception.Error.Message});
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
        }
    }
}