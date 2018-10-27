using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Common;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models.Shared;
using SlackAPI;

namespace MuranoBot.Application.Commands {
	public class SetVacationCommandHandler : IRequestHandler<SetVacationCommand, bool> {
		private readonly VacationsApp _vacationsApp;
		private readonly MessageSender _messageSender;
		private readonly SlackClient _slackClient;

		public SetVacationCommandHandler(VacationsApp vacationsApp, MessageSender messageSender) {
			_vacationsApp = vacationsApp;
			_messageSender = messageSender;
			_slackClient = new SlackClient(AppConfig.Instance.SlackToken);
		}

		public Task<bool> Handle(SetVacationCommand command, CancellationToken cancellationToken) {
			Destination destination = new Destination {
				ChannelId = command.ChannelId,
				UserId = command.UserId,
				Messenger = Messenger.Slack,
			};

			var realName = GetRealName(command.UserId);
			var domainName = ConvertToDomainName(realName.FirstName, realName.LastName);

			try
			{
				_vacationsApp.SetVacation(new VacationInfo
				{
					DomainName = domainName,
					Interval = new TimeInterval(command.From, command.To),
				});
			}
			catch (Exception e)
			{
				_messageSender.SendAsync(destination, new BotResponse() { Text = "К сожалению TimeTracker временно недоступен." });
				return Task.FromResult(true);
			}


			string dateFromFormated = command.From.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
			string dateToFormated = command.To.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
			BotResponse botResponse = 
				 new BotResponse { Text = $"'Для '{realName.FirstName} {realName.LastName}' установлен отпуск с '{dateFromFormated}' по '{dateToFormated}'" };
			_messageSender.SendAsync(destination, botResponse);

			return Task.FromResult(true);
		}

		private (string FirstName, string LastName) GetRealName(string userId) {
			SlackAPI.User user = null;

			var mre = new ManualResetEvent(false);
			_slackClient.GetInfo(response => {
				user = response.user;
				mre.Set();
			}, userId);
			mre.WaitOne();

			return (user.profile.first_name, user.profile.last_name);
		}

		private string ConvertToDomainName(string firstName, string lastName) {
			return @"CORP\"+$"{firstName}.{lastName}";
		}
	}
}
