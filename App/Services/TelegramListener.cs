using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using MuranoBot.Common;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageParsers;
using Messengers.Models;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using MihaZupan;

namespace App.Services
{
    public class TelegramListener : IHostedService
    {
        private IServiceProvider _serviceProvider;
        private TelegramBotClient _telegramBotClient;

        public TelegramListener(IServiceProvider serviceProvider, AppConfig appConfig)
        {
            _serviceProvider = serviceProvider;
            _telegramBotClient = new TelegramBotClient(appConfig.TelegramToken, new HttpToSocks5Proxy(appConfig.TelegramProxyHost, appConfig.TelegramProxyPort));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!START TG");
            _telegramBotClient.OnMessage += OnMessageReceived;
            _telegramBotClient.StartReceiving(Array.Empty<UpdateType>());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _telegramBotClient.StopReceiving();
            return Task.CompletedTask;
        }

        private void OnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            Console.WriteLine(messageEventArgs.Message.Text);
        }
    }
}
