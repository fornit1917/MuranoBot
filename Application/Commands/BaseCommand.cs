using System;
using System.Collections.Generic;
using System.Text;

namespace MuranoBot.Application.Commands {
	public abstract class BaseCommand {
		public string ChannelId { get; private set; }

		public string UserId { get; private set; }

		public BaseCommand(string channelId, string userId) {
			ChannelId = channelId;
			UserId = userId;
		}
	}
}
