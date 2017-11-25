using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;

using SkynetAPI.Services;
using SkynetAPI.Services.Interfaces;
using SkynetAPI.DBContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using SkynetAPI.Configs;

namespace SkynetAPI
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
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"{Configuration["Auth0:Domain"]}";
                options.Audience = Configuration["Auth0:Audience"];
            });

            services.Configure<TableConfig>(config =>
            {
                config.ConnectionString = Configuration["StorageAccount"];
            });
            
            services.AddTransient<IZonesRepository, ZonesRepository>();
            services.AddTransient<IDevicesRepository, DevicesRepository>();
            services.AddTransient<IClientsRepository, ClientsRepository>();
            services.AddTransient<IConfigurationRepository, ConfigurationRepository>();
            services.AddTransient<IUserRepository, UsersRepository>();
            services.AddTransient<IUserMapper, UserMapper>();
            services.AddTransient<IQueueService, QueueService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes => {
                routes.MapRoute(
                    "ADMINRoute",
                    "{area:exists}/{controller=Users}/{action=Index}/{id?}");

                routes.MapRoute(
                    "APIRoute",
                    "{area:exists}/{controller}/{action}/{id?}");
            });
        }
    }
}
