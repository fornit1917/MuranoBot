using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using MediatR;
using MuranoBot.Application.Commands;

namespace MuranoBot.Application.Infrastructure {
	public class MediatorModule : Autofac.Module {
		protected override void Load(ContainerBuilder builder) {
			builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
				.AsImplementedInterfaces();

			var assembly = typeof(CheckVacationCommandHandler).GetTypeInfo().Assembly;
			builder.RegisterAssemblyTypes(assembly)
				.AsClosedTypesOf(typeof(IRequestHandler<,>))
				.AsImplementedInterfaces()
				.InstancePerDependency();

			builder.Register<ServiceFactory>(context => {
				var c = context.Resolve<IComponentContext>();
				return t => c.Resolve(t);
			});
		}
	}
}
