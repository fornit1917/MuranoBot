﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SlackAPI;
using SlackConnector;
using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Messengers.Services;
using Messengers.Models;

namespace App.Services
{
    public class SlackListener : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private AppConfig _config;

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
                    var botRequest = new BotRequest()
                    {
                        Messenger = Messenger.Slack,
                        ChannelId = message.ChatHub.Id,
                        UserId = message.User.Id,
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