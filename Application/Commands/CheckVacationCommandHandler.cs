using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application;

namespace MuranoBot.Application.Commands {
	public class CheckVacationCommandHandler : IRequestHandler<CheckVacationCommand, bool> {
		private readonly VacationsApp _vacationsApp;
		private readonly MessageSender _messageSender;

		public CheckVacationCommandHandler(VacationsApp vacationsApp, MessageSender messageSender) {
			_vacationsApp = vacationsApp;
			_messageSender = messageSender;
		}

		public Task<bool> Handle(CheckVacationCommand command, CancellationToken cancellationToken) {
			Destination destination = new Destination {
				ChannelId = command.ChannelId,
				UserId = command.UserId,
				Messenger = command.Messenger,
			};

			var rc = _vacationsApp.GetVacationInfo(1091, new DateTime(2018, 08, 27));

			BotResponse botResponse;
			if (rc != null) {
				botResponse = new BotResponse { Text = $"'{command.UserId}' в отпуске с '{rc.Interval.Start}' по '{rc.Interval.End}'" };
			}
			else {
				botResponse = new BotResponse { Text = $"'{command.UserId}' не в отпуске" };
			}

			_messageSender.SendAsync(destination, botResponse);

			return Task.FromResult(true);
		}
	}
}
