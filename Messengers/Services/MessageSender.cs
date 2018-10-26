using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlackAPI;
using Messengers.Models;
using Common;
using Domain;

namespace Messengers.Services
{
    public class MessageSender
    {
        private SlackTaskClient _slackClient;
        private bool _isMocked;

        public MessageSender(AppConfig appConfig)
        {
            _slackClient = new SlackTaskClient(appConfig.SlackToken);
            _isMocked = appConfig.MockSender;
        }

        public Task SendAsync(Destination destination, BotResponse botResponse)
        {
            if (_isMocked)
            {
                Console.WriteLine($"{destination.Messenger.ToString()}, {destination.ChannelId}, {destination.UserId}: {botResponse.Text}");
                Console.WriteLine("-------------------------------------------------------");
                return Task.CompletedTask;
            }

            switch (destination.Messenger)
            {
                case Messenger.Slack:
                    return _slackClient.PostMessageAsync(destination.ChannelId, botResponse.Text);
                default:
                    // todo: log warning: unsupported messenger
                    return Task.CompletedTask;
            }
        }

        public Task SendAsync(IEnumerable<Destination> destinations, BotResponse botResponse)
        {
            throw new NotImplementedException();
        }
    }
}
