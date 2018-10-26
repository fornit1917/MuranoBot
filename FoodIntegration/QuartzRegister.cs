using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace FoodIntegration {
	public static class QuartzRegister {
		public static async Task Run() {
			var scheduler = await StdSchedulerFactory.GetDefaultScheduler();
			await scheduler.Start();

			var userEmailsJob = JobBuilder.Create<NewMenuCheckJob>()
				.WithIdentity("NewMenuCheck")
				.Build();
			var userEmailsTrigger = TriggerBuilder.Create()
				.WithIdentity("NewMenuCheckCron")
				.StartNow()
				.WithCronSchedule("0 0 12 ? * MON,TUE,WED,THU,FRI *")
				//.WithCronSchedule("0/10 * * ? * * *")
				.Build();

			scheduler.ScheduleJob(userEmailsJob, userEmailsTrigger).Wait();

			var adminEmailsJob = JobBuilder.Create<OrderIsMadeJob>()
				.WithIdentity("OrderIsMade")
				.Build();
			var adminEmailsTrigger = TriggerBuilder.Create()
				.WithIdentity("OrderIsMadeCron")
				.StartNow()
				.WithCronSchedule("0 0 10/2 ? * MON,TUE,WED,THU,FRI *")
				//.WithCronSchedule("0/10 * * ? * * *")
				.Build();

			await scheduler.ScheduleJob(adminEmailsJob, adminEmailsTrigger);
		}
	}
}
