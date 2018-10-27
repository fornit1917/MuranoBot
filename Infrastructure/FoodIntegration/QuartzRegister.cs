using System.Threading.Tasks;
using Autofac;
using MuranoBot.Common;
using Quartz;

namespace FoodIntegration
{
	public static class QuartzRegister
	{
		public static async Task Run(IContainer container)
		{
			var schedulerFactory = container.Resolve<ISchedulerFactory>();
			var appConfig = container.Resolve<AppConfig>();

			IScheduler scheduler = await schedulerFactory.GetScheduler();
			await scheduler.Start();

			var userEmailsJob = JobBuilder.Create<NewMenuCheckJob>()
				.WithIdentity("NewMenuCheck")
				.Build();
			var userEmailsTrigger = TriggerBuilder.Create()
				.WithIdentity("NewMenuCheckCron")
				.StartNow()
				//.WithCronSchedule("0 0 12 ? * MON,TUE,WED,THU,FRI *")
				.WithCronSchedule(appConfig.NewMenuCheckJobSchedule)
				.Build();

			await scheduler.ScheduleJob(userEmailsJob, userEmailsTrigger);

			var adminEmailsJob = JobBuilder.Create<OrderIsMadeJob>()
				.WithIdentity("OrderIsMade")
				.Build();
			var adminEmailsTrigger = TriggerBuilder.Create()
				.WithIdentity("OrderIsMadeCron")
				.StartNow()
				//.WithCronSchedule("0 0 11/2 ? * MON,TUE,WED,THU,FRI *")
				.WithCronSchedule(appConfig.OrderIsMadeJobSchedule)
				.Build();

			await scheduler.ScheduleJob(adminEmailsJob, adminEmailsTrigger);
		}
	}
}