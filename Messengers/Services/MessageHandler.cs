using Messengers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using SlackAPI;

namespace Messengers.Services
{
    public class MessageHandler
    {
        private MessageSender _messageSender;

        public MessageHandler(MessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public Task HandleRequestAsync(BotRequest botRequest)
        {
            // default destination (sender)
            Destination destination = new Destination() { ChannelId = botRequest.ChannelId, UserId = botRequest.UserId, Messenger = botRequest.Messenger };

            BotResponse botResponse;

            var vacationInfoRequest = VacationInfoRequest.TryParse(botRequest);
            if (vacationInfoRequest != null)
            {
                botResponse = new BotResponse() { Text = "Vacation info request for: " + vacationInfoRequest.Name };
                return _messageSender.SendAsync(destination, botResponse);
            }

            botResponse = new BotResponse() { Text = "Unknown command: " + botRequest.Text };
            return _messageSender.SendAsync(destination, botResponse);
        }


    }
}
