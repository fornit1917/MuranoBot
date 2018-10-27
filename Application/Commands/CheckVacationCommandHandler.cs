using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Common;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models;
using SlackAPI;

namespace MuranoBot.Application.Commands {
	public class CheckVacationCommandHandler : IRequestHandler<CheckVacationCommand, bool> {
		private readonly VacationsApp _vacationsApp;
		private readonly MessageSender _messageSender;
		private readonly SlackClient _slackClient;

		public CheckVacationCommandHandler(VacationsApp vacationsApp, MessageSender messageSender, AppConfig appConfig) {
			_vacationsApp = vacationsApp;
			_messageSender = messageSender;
			_slackClient = new SlackClient(appConfig.SlackToken);
		}

		public Task<bool> Handle(CheckVacationCommand command, CancellationToken cancellationToken) {
			Destination destination = new Destination {
				ChannelId = command.ChannelId,
				UserId = command.UserId,
				Messenger = command.Messenger,
			};

			RealName realName;
			if (command.TargetUser.FullName != null) {
				realName = GetRealNameByTypedName(command.TargetUser.FullName);
			} else if (command.TargetUser.SlackId != null) {
				realName = GetRealName(command.UserId);
			} else {
				_messageSender.SendAsync(destination, new BotResponse() { Text = "Не могу распазнать имя" });
				return Task.FromResult(false);
			}
			var domainName = ConvertToDomainName(realName.FirstName, realName.LastName);

			VacationInfo rc;
			try
			{
				rc = _vacationsApp.GetVacationInfo(domainName, DateTime.UtcNow);
			}
			catch (Exception e)
			{
				_messageSender.SendAsync(destination, new BotResponse() { Text = "Информация об отпусках временно недоступна." });
				return Task.FromResult(true);
			}
			

			BotResponse botResponse;
			if (rc != null) {
				string dateFromFormated = rc.Interval.Start.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
				string dateToFormated = rc.Interval.End.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
				botResponse = new BotResponse { Text = $"'{realName.FirstName} {realName.LastName}' в отпуске с '{dateFromFormated}' по '{dateToFormated}'" };
			} else {
				botResponse = new BotResponse { Text = $"'{realName.FirstName} {realName.LastName}' не в отпуске" };
			}

			_messageSender.SendAsync(destination, botResponse);

			return Task.FromResult(true);
		}

		private RealName GetRealName(string userId) {
			SlackAPI.User user = null;

			var mre = new ManualResetEvent(false);
			_slackClient.GetInfo(response => {
				user = response.user;
				mre.Set();
			}, userId);
			mre.WaitOne();

			return new RealName {FirstName = user.profile.first_name, LastName = user.profile.last_name};
		}

		private RealName GetRealNameByTypedName(string userName)
		{
			var parts = userName.Split(' ');
			parts[0] = parts[0].Trim();
			if (parts.Length > 1)
			{
				parts[1] = parts[1].Trim();
			}

			return new RealName {FirstName = parts[0].Trim(), LastName = parts.Length > 1 ? parts[1].Trim() : ""};
		}

		private string ConvertToDomainName(string firstName, string lastName) {
			return @"CORP\"+$"{firstName}.{lastName}";
		}

		private class RealName {
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}
	}
}
