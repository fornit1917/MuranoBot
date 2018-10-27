using Messengers.Models;
using System;
using System.Threading.Tasks;
using MuranoBot.Infrastructure.TimeTracking.App.Application;
using Domain;
using Common;
using SlackAPI;
using MediatR;
using MuranoBot.Application.Commands;

namespace Messengers.Services
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

        public void HandleRequestAsync(BotRequest botRequest)
        {
            // default destination (sender)

            if (botRequest.IsDirectMessage)
            {
	            //bool isRegistered = await _botRepository.IsLinkRegistered(botRequest.Messenger, botRequest.UserId);
	            //if (!isRegistered)
	            //{
		        //    Guid authToken = await _botRepository.RegisterLink(botRequest.Messenger, botRequest.UserId);
		        //    string link = "http://localhost:55659/api/auth/" + authToken; // todo take from config
				//	botResponse = new BotResponse { Text = $"Перейдите по ссылке {link} для регистрации" };
		        //    await _messageSender.SendAsync(destination, botResponse);
				//	return;
				//}
            }

            var vacationInfoRequest = VacationInfoRequest.TryParse(botRequest);
			if (vacationInfoRequest != null) {
				_mediator.Send(new CheckVacationCommand(botRequest.ChannelId, vacationInfoRequest.Name));
            } else {
				var command = new UnknownCommand(botRequest.ChannelId, botRequest.UserId, botRequest.Text);
				_mediator.Send(command);
			}
        }
    }
}
