using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using App.Services;
using Common;
using Messengers.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;

namespace App
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		public IContainer ApplicationContainer { get; private set; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// config
			var appConfig = AppConfig.Instance;
			Configuration.GetSection("AppConfig").Bind(appConfig);
			services.AddSingleton<AppConfig>(appConfig);

            // hosted services
            if (appConfig.RunSlackBot)
			{
				services.AddHostedService<SlackListener>();
			}

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Autofac
			var builder = new ContainerBuilder();
			builder.Populate(services);

			builder.RegisterType<MessageHandler>()
				.AsSelf()
				.InstancePerLifetimeScope();
			builder.RegisterType<MessageSender>()
				.AsSelf()
				.InstancePerLifetimeScope();
			ApplicationContainer = builder.Build();

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