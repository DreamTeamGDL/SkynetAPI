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

namespace SkynetAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        private readonly IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IZonesRepository, ZonesRepository>();
            services.AddTransient<IDevicesRepository, DevicesRepository>();
            services.AddTransient<IClientsRepository, ClientsRepository>();
            services.AddTransient<IUserMapper, UserMapper>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>{
                routes.MapAreaRoute(
                    "APIRoute",
                    "api",
                    "{area:exists}/{controller}/{action}/{id?}"
                );
            });
        }
    }
}
