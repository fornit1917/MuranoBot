using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Domain;
using Messengers.Models;
using Messengers.Services;
using Quartz;

namespace FoodIntegration
{
	public class NewMenuCheckJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			var foodRepo = new FoodRepository();
			DateTime[] actualMenuDates = TempData.ActualMenuDates;
			DateTime[] newMenuDates = await foodRepo.NewMenuDates(actualMenuDates.Max());
			if (newMenuDates.Length > 0)
			{
				TempData.ActualMenuDates = newMenuDates;
				var sender = new MessageSender(AppConfig.Instance);
				string message = $"Меню на {newMenuDates.Min().ToShortDateString()} - {newMenuDates.Max().ToShortDateString()} выложено";
				await sender.SendAsync(
					new[]{new Destination {Messenger = Messenger.Slack, ChannelId = "C5WDTD3QC" }}, //todo move id to config, add messengers
					new BotResponse {Text = message});
			}
		}
	}
}