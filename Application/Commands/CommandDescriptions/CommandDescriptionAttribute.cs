using System;

namespace MuranoBot.Application.Commands.CommandDescriptions
{
	[AttributeUsage(AttributeTargets.Field)]
	public class CommandDescriptionAttribute : Attribute
	{
		public string Command { get; set; }
		public string Hint { get; set; }
	}
}