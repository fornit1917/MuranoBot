using System;
using System.Linq;
using System.Threading.Tasks;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;
using Quartz;

namespace FoodIntegration
{
	public class NewMenuCheckJob : IJob
	{
		private readonly FoodRepository _foodRepository;
		private readonly MessageSender _messageSender;

		public NewMenuCheckJob(FoodRepository foodRepository, MessageSender messageSender)
		{
			_foodRepository = foodRepository;
			_messageSender = messageSender;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			DateTime[] actualMenuDates = TempData.ActualMenuDates;
			DateTime[] newMenuDates = await _foodRepository.NewMenuDates(actualMenuDates.Max());
			if (newMenuDates.Length > 0)
			{
				TempData.ActualMenuDates = newMenuDates;
				string message = $"Меню на {newMenuDates.Min().ToShortDateString()} - {newMenuDates.Max().ToShortDateString()} выложено";
				await _messageSender.SendAsync(
					new[]{new Destination {Messenger = Messenger.Slack, ChannelId = "C5WDTD3QC" }}, //todo move id to config, add messengers
					new BotResponse {Text = message});
			}
		}
	}
}