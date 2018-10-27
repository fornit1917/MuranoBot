using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Domain;
using MuranoBot.Application.Models;

namespace MuranoBot.Application.Commands {
	public class CheckVacationCommand : BaseCommand, IRequest<bool> {
		public UserInfo TargetUser { get; private set; }

		public CheckVacationCommand(Messenger sourceMessenger, string channelId, string userId, UserInfo targetUser) : base(sourceMessenger, channelId, userId) {
			TargetUser = targetUser;
		}
	}
}
