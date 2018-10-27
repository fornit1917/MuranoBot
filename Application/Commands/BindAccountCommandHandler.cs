using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Common;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;

namespace MuranoBot.Application.Commands
{
	public class BindAccountCommandHandler : IRequestHandler<BindAccountCommand, bool>
	{
		private readonly MessageSender _messageSender;
		private readonly BotRepository _botRepository;
		private readonly AppConfig _appConfig;

		public BindAccountCommandHandler(MessageSender messageSender, BotRepository botRepository, AppConfig appConfig)
		{
			_messageSender = messageSender;
			_botRepository = botRepository;
			_appConfig = appConfig;
		}

		public async Task<bool> Handle(BindAccountCommand request, CancellationToken cancellationToken)
		{
			Guid authToken = await _botRepository.RegisterLink(request.Messenger, request.UserId);
			string link = _appConfig.BindAccountUrl + authToken;
			var destination = new Destination { Messenger = request.Messenger, ChannelId = request.ChannelId, UserId = request.UserId };
			await _messageSender.SendAsync(destination, new BotResponse { Text = $"Перейдите по ссылке {link} для регистрации" });

			return true;
		}
	}
}