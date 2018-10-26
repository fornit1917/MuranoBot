using Messengers.Models;
using System;
using System.Threading.Tasks;
using MuranoBot.TimeTracking.App.Application;
using Domain;

namespace Messengers.Services
{
    public class MessageHandler
    {
        private readonly MessageSender _messageSender;
		private readonly VacationsApp _vacationsApp;
		private readonly BotRepository _botRepository;

        public MessageHandler(MessageSender messageSender, VacationsApp vacationsApp, BotRepository botRepository)
        {
            _messageSender = messageSender;
			_vacationsApp = vacationsApp;
			_botRepository = botRepository;
        }

        public async Task HandleRequestAsync(BotRequest botRequest)
        {
            // default destination (sender)
            Destination destination = new Destination() { ChannelId = botRequest.ChannelId, UserId = botRequest.UserId, Messenger = botRequest.Messenger };
            BotResponse botResponse;

            if (botRequest.IsDirectMessage)
            {
	            bool isRegistered = await _botRepository.IsLinkRegistered(botRequest.Messenger, botRequest.ChannelId);
	            if (!isRegistered)
	            {
		            Guid authToken = await _botRepository.RegisterLink(botRequest.Messenger, botRequest.ChannelId);
		            string link = "http://localhost:55659/api/auth/" + authToken; // todo take from config
					botResponse = new BotResponse { Text = $"Перейдите по ссылке {link} для регистрации" };
		            await _messageSender.SendAsync(destination, botResponse);
					return;
				}
            }

            var vacationInfoRequest = VacationInfoRequest.TryParse(botRequest);
            if (vacationInfoRequest != null)
            {
                botResponse = new BotResponse { Text = "Запрос на информацию об отпуске у " + vacationInfoRequest.Name };
                await _messageSender.SendAsync(destination, botResponse);
            }

            botResponse = new BotResponse { Text = $"Команду '{botRequest.Text}' я не знаю" };
            await _messageSender.SendAsync(destination, botResponse);
        }
    }
}
