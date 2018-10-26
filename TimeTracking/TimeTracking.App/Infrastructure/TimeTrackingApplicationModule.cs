using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using MuranoBot.TimeTracking.App.Application;
using MuranoBot.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.TimeTracking.App.Infrastructure {
	public class TimeTrackingApplicationModule : Module {

		protected override void Load(ContainerBuilder builder) {
			builder.RegisterType<TimeTrackerDbContext>()
				.AsSelf()
				.InstancePerLifetimeScope();

			builder.RegisterType<VacationsRepository>()
				.AsSelf()
				.InstancePerLifetimeScope();
			builder.RegisterType<VacationsApp>()
				.AsSelf()
				.InstancePerLifetimeScope();

			builder.RegisterType<UsersRepository>()
				.AsSelf()
				.InstancePerLifetimeScope();
			builder.RegisterType<UsersApp>()
				.AsSelf()
				.InstancePerLifetimeScope();
		}
	}
}
