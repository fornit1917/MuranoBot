using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace MuranoBot.Application.Commands {
	public class SetVacationCommand : BaseCommand, IRequest<bool> {
		public DateTime From { get; private set; }
		public DateTime To { get; private set; }

		public SetVacationCommand(string channelId, string userId, DateTime from, DateTime to) : base(channelId, userId) {
			From = from;
			To = to;
		}
	}
}
