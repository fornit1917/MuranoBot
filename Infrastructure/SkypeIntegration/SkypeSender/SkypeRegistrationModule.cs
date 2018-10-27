using Autofac;
using SkypeIntegration.Skype;

namespace SkypeIntegration {
	public class SkypeRegistrationModule : Module {

		protected override void Load(ContainerBuilder builder) {
			builder.RegisterType<SkypeSender>()
				.AsSelf()
				.SingleInstance();
		}
	}
}
