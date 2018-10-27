using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SlackConnector;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Domain;
using Messengers.Services;
using Messengers.Models;

namespace App.Services
{
    public class SlackListener : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AppConfig _config;

        public SlackListener(IServiceProvider serviceProvider, AppConfig config)
        {
            _config = config;
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var connector = new SlackConnector.SlackConnector();
            ISlackConnection conn = await connector.Connect(_config.SlackToken);
            conn.OnMessageReceived += message =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var botRequest = new BotRequest
                    {
                        Messenger = Messenger.Slack,
                        ChannelId = message.ChatHub.Id,
                        UserId = message.User.Id,
                        IsDirectMessage = message.ChatHub.Type == SlackConnector.Models.SlackChatHubType.DM,
                        Text = message.Text,
                    };

                    MessageHandler messageHandler = scope.ServiceProvider.GetService<MessageHandler>();

                    return messageHandler.HandleRequestAsync(botRequest);
                }
            };
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}