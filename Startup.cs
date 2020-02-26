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
using Blogger.Extensions;
using Blogger.Options;
using Blogger.Data;
using Blogger.Services;

namespace Blogger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            DbOptions dbOptions = new DbOptions();
            Configuration.GetSection(nameof(DbOptions)).Bind(dbOptions);
            AppOptionProvider.DbOptions = dbOptions;

            SwaggerOptions swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            AppOptionProvider.SwaggerOptions = swaggerOptions;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<BloggerContext>();
            services.AddScoped<PostRepository>();
            services.AddScoped<PostService>();
            services.AddControllers();
            services.ConfigureSwaggerPage();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCustomExceptionHandler();
            
            var swaggerOptions = AppOptionProvider.SwaggerOptions;
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
