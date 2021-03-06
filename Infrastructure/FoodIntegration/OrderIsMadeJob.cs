﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MuranoBot.Common;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;
using Quartz;

namespace FoodIntegration
{
	public class OrderIsMadeJob : IJob
	{
		private readonly FoodRepository _foodRepository;
		private readonly BotRepository _botRepository;
		private readonly MessageSender _messageSender;
		private readonly AppConfig _appConfig;

		public OrderIsMadeJob(FoodRepository foodRepository, BotRepository botRepository, MessageSender messageSender, AppConfig appConfig)
		{
			_foodRepository = foodRepository;
			_botRepository = botRepository;
			_messageSender = messageSender;
			_appConfig = appConfig;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			DateTime[] actualMenuDates = await _botRepository.GetActualMenuDates();
			Dictionary<string, DateTime[]> noOrdersForDatesByUser = await _foodRepository.NoOrderDatesByUserEmail(actualMenuDates);
			if (noOrdersForDatesByUser.Count > 0)
			{				
				ILookup<string, (Messenger Messenger, string ExternalId)> externalIds = await _botRepository.GetExternalIdByUserEmail(noOrdersForDatesByUser.Keys);
				foreach (IGrouping<string, (Messenger Messenger, string ExternalId)> externalIdGroup in externalIds)
				{
					string dates = string.Join(", ", noOrdersForDatesByUser[externalIdGroup.Key].Select(d => d.ToShortDateString()));
					string message = $"Еда не заказана на {dates}. Это можно исправить тут {_appConfig.FoodMenuLink}";
					Destination[] destinations = externalIdGroup.Select(g => new Destination {
						Messenger = g.Messenger,
						UserId = g.ExternalId,
						ChannelId = g.ExternalId
					}).ToArray();
					await _messageSender.SendAsync(destinations, new BotResponse {Text = message});
				}
			}
		}
	}
}