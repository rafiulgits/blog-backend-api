using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Blogger.Options;

namespace Blogger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            var swaggerOptions = new SwaggerOptions();
            Configuration.GetSection("SwaggerOptions").Bind(swaggerOptions);
            app.UseSwagger(op => {op.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(op => {op.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
