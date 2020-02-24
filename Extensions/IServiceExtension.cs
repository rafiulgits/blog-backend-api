using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Blogger.Extensions
{
    public static class IServiceExtension
    {
        public static void ConfigureSwaggerPage(this IServiceCollection services)
        {
            services.AddSwaggerGen(op =>{
                op.SwaggerDoc("v1", new OpenApiInfo{Title="API", Version="v1"});

                op.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
                    Description = "JWT Authorization header using the Bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                op.AddSecurityRequirement(new OpenApiSecurityRequirement{
                   {
                       new OpenApiSecurityScheme {
                           Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id="Bearer"}
                       },
                       new string[] {}
                   }
                });
            });
        }
    }
}