using Messengers.Models;
using System;
using System.Threading.Tasks;
using MuranoBot.TimeTracking.App.Application;
using Domain;
using Common;
using SlackAPI;
using MediatR;
using MuranoBot.Application.Commands;

namespace Messengers.Services
{
    public class MessageHandler
    {
        private readonly MessageSender _messageSender;
		private readonly IMediator _mediator;
		private readonly BotRepository _botRepository;

        public MessageHandler(MessageSender messageSender, IMediator mediator, BotRepository botRepository)
        {
            _messageSender = messageSender;
			_mediator = mediator;
			_botRepository = botRepository;
        }

        public async Task HandleRequestAsync(BotRequest botRequest)
        {
            // default destination (sender)
            Destination destination = new Destination { ChannelId = botRequest.ChannelId, UserId = botRequest.UserId, Messenger = botRequest.Messenger };
            BotResponse botResponse;

            if (botRequest.IsDirectMessage)
            {
	            bool isRegistered = await _botRepository.IsLinkRegistered(botRequest.Messenger, botRequest.UserId);
	            if (!isRegistered)
	            {
		            Guid authToken = await _botRepository.RegisterLink(botRequest.Messenger, botRequest.UserId);
		            string link = "http://localhost:55659/api/auth/" + authToken; // todo take from config
					botResponse = new BotResponse { Text = $"Перейдите по ссылке {link} для регистрации" };
		            await _messageSender.SendAsync(destination, botResponse);
					return;
				}
            }

            var vacationInfoRequest = VacationInfoRequest.TryParse(botRequest);
            if (vacationInfoRequest != null)
            {
				_mediator.Send(new CheckVacationCommand(botRequest.ChannelId, vacationInfoRequest.Name));
                botResponse = new BotResponse { Text = "Запрос на информацию об отпуске у " + vacationInfoRequest.Name };
                await _messageSender.SendAsync(destination, botResponse);
            }

            botResponse = new BotResponse { Text = $"Команду '{botRequest.Text}' я не знаю" };
            await _messageSender.SendAsync(destination, botResponse);
        }
    }
}
