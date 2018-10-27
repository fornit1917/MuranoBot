using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Domain;

namespace MuranoBot.Application.Commands {
	public class CheckVacationCommand : BaseCommand, IRequest<bool> {
		public string Channel { get; private set; }
		public string UserName { get; private set; }

		public CheckVacationCommand(Messenger sourceMessenger, string channelId, string userId, string channel, string userName) : base(sourceMessenger, channelId, userId) {
			Channel = channel;
			UserName = userName;
		}
	}
}
