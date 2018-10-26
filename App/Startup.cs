using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messengers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using App.Services;
using Common;
using Messengers.Services;

namespace App
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
            // config
            var appConfig = new AppConfig();
            Configuration.GetSection("AppConfig").Bind(appConfig);
            services.AddSingleton<AppConfig>(appConfig);

            // scoped services
            services.AddScoped<MessageHandler>();
            services.AddScoped<MessageSender>();

            // hosted services
            if (appConfig.RunSlackBot)
            {
                services.AddHostedService<SlackListener>();
            }
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
