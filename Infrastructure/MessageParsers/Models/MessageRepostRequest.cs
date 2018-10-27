using Messengers.Models;
using MuranoBot.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using MuranoBot.Application.Commands;
using MuranoBot.Application.Commands.CommandDescriptions;

namespace MessageParsers.Models
{
    class MessageRepostRequest
    {
        public static bool IsRepostRequest(BotRequest botRequest)
        {
            return botRequest.Messenger == Messenger.Slack && botRequest.Text.StartsWith(Command.Repost.GetCommandText());
        }
    }
}
