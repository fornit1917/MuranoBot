using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MediatR;
using MuranoBot.Domain;

namespace MuranoBot.Application.Commands {
	public class UnknownCommand : BaseCommand, IRequest<bool> {
		[DataMember]
		public string Text { get; private set; }

		public UnknownCommand(Messenger sourceMessenger, string channelId, string userId, string text) : base(sourceMessenger, channelId, userId) {
			Text = text;
		}
	}
}
