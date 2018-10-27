using System;
using System.Linq;
using System.Threading.Tasks;
using MuranoBot.Common;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;
using Quartz;

namespace FoodIntegration
{
	public class NewMenuCheckJob : IJob
	{
		private readonly FoodRepository _foodRepository;
		private readonly BotRepository _botRepository;
		private readonly MessageSender _messageSender;
		private readonly AppConfig _appConfig;

		public NewMenuCheckJob(FoodRepository foodRepository, BotRepository botRepository, MessageSender messageSender, AppConfig appConfig)
		{
			_foodRepository = foodRepository;
			_botRepository = botRepository;
			_messageSender = messageSender;
			_appConfig = appConfig;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			DateTime[] actualMenuDates = await _botRepository.GetActualMenuDates();
			DateTime[] newMenuDates = await _foodRepository.NewMenuDates(actualMenuDates.Max());
			if (newMenuDates.Length > 0)
			{
				await _botRepository.SaveActualMenuDates(newMenuDates);
				string message = $"Меню на {newMenuDates.Min().ToShortDateString()} - {newMenuDates.Max().ToShortDateString()} выложено";
				await _messageSender.SendAsync(
					new[]
					{
						new Destination {Messenger = Messenger.Slack, ChannelId = _appConfig.SlackOfficeChat },
						new Destination {Messenger = Messenger.Skype, ChannelId = _appConfig.SkypeOfficeChat} 
					},
					new BotResponse {Text = message});
			}
		}
	}
}