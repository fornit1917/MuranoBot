using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SlackAPI;
using MuranoBot.Infrastructure.SlackAPI.Models;
using Common;
using Domain;

namespace MuranoBot.Infrastructure.SlackAPI
{
    public class MessageSender
    {
        private readonly SlackTaskClient _slackClient;
        private readonly bool _isMocked;

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
	        return Task.WhenAll(destinations.Select(d => SendAsync(d, botResponse)));
        }
    }
}
