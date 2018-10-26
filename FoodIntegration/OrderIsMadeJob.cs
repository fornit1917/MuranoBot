using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Domain;
using Messengers.Models;
using Messengers.Services;
using Quartz;

namespace FoodIntegration
{
	public class OrderIsMadeJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			var foodRepo = new FoodRepository();
			DateTime[] actualMenuDates = TempData.ActualMenuDates;
			Dictionary<string, DateTime[]> noOrdersForDatesByUser = await foodRepo.NoOrderDatesByUserEmail(actualMenuDates);
			if (noOrdersForDatesByUser.Count > 0)
			{				
				var botRepo = new BotRepository();
				ILookup<string, (Messenger Messenger, string ExternalId)> externalIds = await botRepo.GetExternalIdByUserEmail(noOrdersForDatesByUser.Keys);
				var sender = new MessageSender(AppConfig.Instance);
				foreach (IGrouping<string, (Messenger Messenger, string ExternalId)> externalIdGroup in externalIds)
				{
					string dates = string.Join(", ", noOrdersForDatesByUser[externalIdGroup.Key].Select(d => d.ToShortDateString()));
					string message = $"Еда не заказана на {dates}. Это можно исправить тут {AppConfig.Instance.FoodMenuLink}";
					Destination[] destinations = externalIdGroup.Select(g => new Destination { Messenger = g.Messenger, ChannelId = g.ExternalId }).ToArray();
					await sender.SendAsync(destinations, new BotResponse {Text = message});
				}
			}
		}
	}
}