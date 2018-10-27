using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Common;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application;
using SlackAPI;

namespace MuranoBot.Application.Commands {
	public class CheckVacationCommandHandler : IRequestHandler<CheckVacationCommand, bool> {
		private readonly VacationsApp _vacationsApp;
		private readonly MessageSender _messageSender;
		private readonly SlackClient _slackClient;

		public CheckVacationCommandHandler(VacationsApp vacationsApp, MessageSender messageSender) {
			_vacationsApp = vacationsApp;
			_messageSender = messageSender;
			_slackClient = new SlackClient(AppConfig.Instance.SlackToken);
		}

		public Task<bool> Handle(CheckVacationCommand command, CancellationToken cancellationToken) {
			Destination destination = new Destination {
				ChannelId = command.ChannelId,
				UserId = command.UserId,
				Messenger = command.Messenger,
			};

			var realName = GetRealName(command.UserId);
			var domainName = ConvertToDomainName(realName.FirstName, realName.LastName);

			var rc = _vacationsApp.GetVacationInfo(domainName, new DateTime(2018, 08, 27));

			BotResponse botResponse;
			if (rc != null) {
				botResponse = new BotResponse { Text = $"'{realName.FirstName} {realName.LastName}' в отпуске с '{rc.Interval.Start}' по '{rc.Interval.End}'" };
			} else {
				botResponse = new BotResponse { Text = $"'{realName.FirstName} {realName.LastName}' не в отпуске" };
			}

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
