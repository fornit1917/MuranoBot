using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MediatR;
using MuranoBot.Domain;

namespace MuranoBot.Application.Commands
{
    public class RepostCommand : BaseCommand, IRequest<bool>
    {
        public string Text { get; set; }
        public Messenger SourceMessenger { get; set; }

        public RepostCommand(Messenger sourceMessenger, string sourceChannnelId, string sourceUserId, string text) : base(sourceMessenger, sourceChannnelId, sourceUserId)
        {
            Text = text;
        }
    }
}
