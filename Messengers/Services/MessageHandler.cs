using Messengers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using SlackAPI;
using MediatR;
using MuranoBot.Application.Commands;

namespace Messengers.Services
{
    public class MessageHandler
    {
        private MessageSender _messageSender;
		private readonly IMediator _mediator;

        public MessageHandler(MessageSender messageSender, IMediator mediator)
        {
            _messageSender = messageSender;
			_mediator = mediator;
        }

        public Task HandleRequestAsync(BotRequest botRequest)
        {
            // default destination (sender)
            Destination destination = new Destination() { ChannelId = botRequest.ChannelId, UserId = botRequest.UserId, Messenger = botRequest.Messenger };
            BotResponse botResponse;

            if (botRequest.IsDirectMessage)
            {
                // botRequest.ChannelId - это наш "ExternalUserId"
                // проверяем, привязан ли по нему и по botRequest.Messenger аккаунт
                // если нет - предлагаем привязать вот так:

                // botResponse = new BotResponse() { Text = "Перейди по ссылке..." };
                // return _messageSender.SendAsync(destination, botResponse);

                // если привязан, то всё ок, ничего не делаем, идём дальше
            }

            var vacationInfoRequest = VacationInfoRequest.TryParse(botRequest);
            if (vacationInfoRequest != null)
            {
				_mediator.Send(new CheckVacationCommand(botRequest.ChannelId, vacationInfoRequest.Name));

                botResponse = new BotResponse() { Text = "Vacation info request for: " + vacationInfoRequest.Name };
				return _messageSender.SendAsync(destination, botResponse);
            }

            botResponse = new BotResponse() { Text = "Unknown command: " + botRequest.Text };
            return _messageSender.SendAsync(destination, botResponse);
        }


    }
}
