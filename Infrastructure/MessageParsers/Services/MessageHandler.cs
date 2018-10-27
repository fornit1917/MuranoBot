using Messengers.Models;
using System;
using System.Threading.Tasks;
using MuranoBot.Domain;
using MediatR;
using MuranoBot.Application.Commands;
using MessageParsers.Models;

namespace MuranoBot.Infrastructure.MessageParsers
{
    public class MessageHandler
    {
		private readonly IMediator _mediator;
		private readonly BotRepository _botRepository;

        public MessageHandler(IMediator mediator, BotRepository botRepository)
        {
			_mediator = mediator;
			_botRepository = botRepository;
        }

        public async Task HandleRequestAsync(BotRequest botRequest)
        {
            // default destination (sender)

            if (botRequest.IsDirectMessage)
            {
				bool isRegistered = await _botRepository.IsLinkRegistered(botRequest.Messenger, botRequest.UserId);
				if (!isRegistered) {
					Guid authToken = await _botRepository.RegisterLink(botRequest.Messenger, botRequest.UserId);
					string link = "http://localhost:55659/api/auth/" + authToken; // todo take from config
					//await _messageSender.SendAsync(destination, new BotResponse { Text = $"Перейдите по ссылке {link} для регистрации" });
					return;
				}
			}

	        var isSuccess = await TryRunCheckVacationCommand(botRequest);
            if (isSuccess)
            {
                return;
            }

            isSuccess = await TryRunSetVacationCommand(botRequest);
            if (isSuccess)
            {
                return;
            }

            isSuccess = await TryRunRepostCommand(botRequest);
            if (isSuccess)
            {
                return;
            }

            var command = new UnknownCommand(botRequest.ChannelId, botRequest.UserId, botRequest.Text);
            await _mediator.Send(command);
        }

        private async Task<bool> TryRunCheckVacationCommand(BotRequest botRequest)
        {
            var vacationInfoRequest = VacationInfoRequest.TryParse(botRequest);
            if (vacationInfoRequest != null)
            {
                return await _mediator.Send(new CheckVacationCommand(botRequest.ChannelId, botRequest.UserId, vacationInfoRequest.Name));
            }
            return false;
        }

        private async Task<bool> TryRunRepostCommand(BotRequest botRequest)
        {
            if (MessageRepostRequest.IsRepostRequest(botRequest))
            {
                var command = new RepostCommand(botRequest.Messenger, botRequest.ChannelId, botRequest.UserId, botRequest.Text);
                return await _mediator.Send(command);
            }
            return false;
        }
        private async Task<bool> TryRunSetVacationCommand(BotRequest botRequest)
        {
            var request = SetVacationRequest.TryParse(botRequest);
            if (request != null)
            {
	            return await _mediator.Send(new SetVacationCommand(botRequest.ChannelId, botRequest.UserId, request.From, request.To));
            }
            return false;
        }
    }
}
