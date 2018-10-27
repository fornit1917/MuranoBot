﻿using System.Threading.Tasks;
using Autofac;
using Quartz;

namespace FoodIntegration
{
	public static class QuartzRegister
	{
		public static async Task Run(IContainer container)
		{
			var schedulerF = container.Resolve<ISchedulerFactory>();
			var scheduler = await schedulerF.GetScheduler();
			await scheduler.Start();

			var userEmailsJob = JobBuilder.Create<NewMenuCheckJob>()
				.WithIdentity("NewMenuCheck")
				.Build();
			var userEmailsTrigger = TriggerBuilder.Create()
				.WithIdentity("NewMenuCheckCron")
				.StartNow()
				//.WithCronSchedule("0 0 12 ? * MON,TUE,WED,THU,FRI *")
				.WithCronSchedule("0/10 * * ? * * *")
				.Build();

			await scheduler.ScheduleJob(userEmailsJob, userEmailsTrigger);

			var adminEmailsJob = JobBuilder.Create<OrderIsMadeJob>()
				.WithIdentity("OrderIsMade")
				.Build();
			var adminEmailsTrigger = TriggerBuilder.Create()
				.WithIdentity("OrderIsMadeCron")
				.StartNow()
				//.WithCronSchedule("0 0 11/2 ? * MON,TUE,WED,THU,FRI *")
				.WithCronSchedule("10/10 * * ? * * *")
				.Build();

			await scheduler.ScheduleJob(adminEmailsJob, adminEmailsTrigger);
		}
	}
}