using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Application.Commands.CommandDescriptions;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;

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
			var text = $"Команду '{command.Text}' я не знаю. Я молод и пока только учусь. Попроси меня о другом: \r\n" +
			           string.Join("\r\n", CommandUtil.GetAllCommands().Select(c => c.GetCommandDescription()).Select(cd => $"\t* '{cd.Command}': {cd.Hint ?? string.Empty}"));
			var botResponse = new BotResponse { Text = text };

			_messageSender.SendAsync(destination, botResponse);

			return Task.FromResult(true);
		}
	}
}
