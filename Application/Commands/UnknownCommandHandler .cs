using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain;
using MuranoBot.Infrastructure.SlackAPI;
using MuranoBot.Infrastructure.SlackAPI.Models;

namespace MuranoBot.Application.Commands {
	public class UnknownCommandHandler : IRequestHandler<UnknownCommand, bool> {
		private readonly MessageSender _messageSender;
		public UnknownCommandHandler(MessageSender messageSender) {
			_messageSender = messageSender;
		}

		public Task<bool> Handle(UnknownCommand command, CancellationToken cancellationToken) {
            Destination destination = new Destination {
				ChannelId = command.ChannelId,
				UserId = command.UserId,
				Messenger = Messenger.Slack,
			};
			var botResponse = new BotResponse { Text = $"Команду '{command.Text}' я не знаю" };

			_messageSender.SendAsync(destination, botResponse);

			return Task.FromResult(true);
		}
	}
}
