using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Domain;
using MuranoBot.Infrastructure.MessageSenders;
using MuranoBot.Infrastructure.MessageSenders.Models;
using MuranoBot.Common;

namespace MuranoBot.Application.Commands
{
    class RepostCommandHandler : IRequestHandler<RepostCommand, bool>
    {
        private readonly MessageSender _messageSender;
        private readonly AppConfig _appConfig;

        public RepostCommandHandler(MessageSender messageSender, AppConfig appConfig)
        {
            _messageSender = messageSender;
            _appConfig = appConfig;
        }

        public Task<bool> Handle(RepostCommand request, CancellationToken cancellationToken)
        {
            var destination = new Destination() { ChannelId = _appConfig.SkypeOfficeChat, UserId = "", Messenger = Messenger.Skype };
            var botResponse = new BotResponse() { Text = request.Text };
            _messageSender.SendAsync(destination, botResponse);
            return Task.FromResult(true);
        }
    }
}
