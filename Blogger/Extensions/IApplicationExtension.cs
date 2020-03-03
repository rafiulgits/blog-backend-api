using System.Text.Json;
using Blogger.Options;
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

        public static void UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            var swaggerOptions = AppOptionProvider.SwaggerOptions;
            app.UseSwagger(op => { op.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(op => { op.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description); });
        }
    }
}