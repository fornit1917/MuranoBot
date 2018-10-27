using Autofac;
using SkypeIntegration.Skype;

namespace SkypeIntegration {
	public class SkypeApplicationModule : Module {

		protected override void Load(ContainerBuilder builder) {
			builder.RegisterType<SkypeSender>()
				.AsSelf()
				.SingleInstance();
		}
	}
}
