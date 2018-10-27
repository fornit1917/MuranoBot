using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using MediatR;
using MuranoBot.Domain;

namespace MuranoBot.Application.Commands
{
	public class BindAccountCommand : BaseCommand, IRequest<bool>
	{
		public BindAccountCommand(Messenger sourceMessenger, string sourceChannnelId, string sourceUserId) : base(sourceMessenger, sourceChannnelId, sourceUserId)
		{
		}
	}
}
