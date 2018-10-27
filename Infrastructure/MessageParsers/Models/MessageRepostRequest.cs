using Messengers.Models;
using MuranoBot.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageParsers.Models
{
    class MessageRepostRequest
    {
        public static bool IsRepostRequest(BotRequest botRequest)
        {
            return botRequest.Messenger == Messenger.Slack && botRequest.Text.Contains("/repost");
        }
    }
}
