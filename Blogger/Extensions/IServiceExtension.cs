using Blogger.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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

        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            var JwtOptions = AppOptionProvider.JwtOptions;

            services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(op =>
            {
                op.SaveToken = true;
                op.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
        }

        public static void AddCorsOnPolicy(this IServiceCollection services, string name)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name,
                builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }
    }
}