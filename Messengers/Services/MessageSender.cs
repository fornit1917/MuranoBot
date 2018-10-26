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

        public MessageSender(AppConfig appConfig)
        {
            _slackClient = new SlackTaskClient(appConfig.SlackToken);
        }

        public Task SendAsync(Destination destination, BotResponse botResponse)
        {
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
