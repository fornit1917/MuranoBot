using Autofac;
using MuranoBot.Infrastructure.TimeTracking.App.Application;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.Infrastructure.TimeTracking.App.Infrastructure {
	public class TimeTrackingRegistrationModule : Module {

		protected override void Load(ContainerBuilder builder) {
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
