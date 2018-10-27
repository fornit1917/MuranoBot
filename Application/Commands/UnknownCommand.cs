using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MediatR;

namespace MuranoBot.Application.Commands {
	public class UnknownCommand : IRequest<bool> {
		[DataMember]
        public string ChannelId { get; private set; }

		[DataMember]
        public string UserId { get; private set; }

		[DataMember]
		public string Text { get; private set; }

		public UnknownCommand(string channelId, string userId, string text) {
			ChannelId = channelId;
			UserId = userId;
			Text = text;
		}
	}
}
