using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Services;
using Common;
using Domain;
using Messengers.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure;
using MuranoBot.Application.Infrastructure;
using SkypeIntegration;

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
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			// config
			var appConfig = AppConfig.Instance;
			Configuration.GetSection("AppConfig").Bind(appConfig);
			services.AddSingleton<AppConfig>(appConfig);

			// scoped services
			services.AddScoped<MessageHandler>();
            services.AddScoped<MessageSender>();
			services.AddScoped<BotRepository>();

            // hosted services
            if (appConfig.RunSlackBot)
			{
				services.AddHostedService<SlackListener>();
			}

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Autofac
			var builder = new ContainerBuilder();
			builder.Populate(services);
			builder.RegisterModule(new TimeTrackingApplicationModule());
			builder.RegisterModule(new MediatorModule());
			builder.RegisterModule(new SkypeApplicationModule());
			return new AutofacServiceProvider(builder.Build());
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
