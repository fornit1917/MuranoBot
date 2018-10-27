using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Services;
using MuranoBot.Common;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FoodIntegration;
using Microsoft.EntityFrameworkCore;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure;
using MuranoBot.Infrastructure.MessageParsers;
using MuranoBot.Infrastructure.MessageSenders;
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
		public IContainer Container { get; private set; }

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

			services.AddDbContext<FoodDbContext>(options => options.UseSqlServer(appConfig.FoodConnectionString));
			services.AddDbContext<TimeTrackerDbContext>(options => options.UseSqlServer(appConfig.TimeTrackerConnectionString));
			services.AddDbContext<DomainDbContext>(options => options.UseSqlServer(appConfig.MainConnectionString));

			// Autofac
			var builder = new ContainerBuilder();
			builder.Populate(services);
			builder.RegisterModule<TimeTrackingRegistrationModule>();
			builder.RegisterModule<FoodIntegrationRegistrationModule>();
			builder.RegisterModule<MediatorRegistrationModule>();
			builder.RegisterModule<SkypeRegistrationModule>();
			Container = builder.Build();
			return new AutofacServiceProvider(Container);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseMvc();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope()) {
				var context = serviceScope.ServiceProvider.GetRequiredService<DomainDbContext>();
				context.Database.EnsureCreated();
			}			
			QuartzRegister.Run(Container).GetAwaiter().GetResult();
		}
	}
}
