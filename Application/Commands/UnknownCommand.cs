using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MediatR;

namespace MuranoBot.Application.Commands {
	public class UnknownCommand : BaseCommand, IRequest<bool> {
		[DataMember]
		public string Text { get; private set; }

		public UnknownCommand(string channelId, string userId, string text) : base(channelId, userId) {
			Text = text;
		}
	}
}
