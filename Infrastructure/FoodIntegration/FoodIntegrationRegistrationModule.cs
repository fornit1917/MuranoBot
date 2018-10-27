using System.Reflection;
using Autofac;
using Autofac.Extras.Quartz;
using Module = Autofac.Module;

namespace FoodIntegration
{
	public class FoodIntegrationRegistrationModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<FoodRepository>().AsSelf().InstancePerDependency();
			builder.RegisterModule<QuartzAutofacFactoryModule>();
			builder.RegisterModule(new QuartzAutofacJobsModule(Assembly.GetExecutingAssembly()));
		}
	}
}