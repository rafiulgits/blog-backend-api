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
            MapConfigurations();
        }

        private void MapConfigurations()
        {
            MapDbCongfiguration();
            MapSwaggerCongfiguration();
            MapJwtConfiguration();
        }

        private void MapDbCongfiguration()
        {
            DbOptions dbOptions = new DbOptions();
            Configuration.GetSection(nameof(DbOptions)).Bind(dbOptions);
            AppOptionProvider.DbOptions = dbOptions;
        }

        private void MapSwaggerCongfiguration()
        {
            SwaggerOptions swaggerOptions = new SwaggerOptions();
            Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);
            AppOptionProvider.SwaggerOptions = swaggerOptions;
        }

        private void MapJwtConfiguration()
        {
            JwtOptions jwtOptions = new JwtOptions();
            Configuration.GetSection(nameof(JwtOptions)).Bind(jwtOptions);
            AppOptionProvider.JwtOptions = jwtOptions;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<BloggerContext>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddControllers(options => 
            {
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true;
            }).AddXmlSerializerFormatters();
            services.AddJwtAuthentication();
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

            app.UseSwaggerMiddleware();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
