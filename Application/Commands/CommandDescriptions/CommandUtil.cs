using System;
using System.Linq;
using System.Reflection;

namespace MuranoBot.Application.Commands.CommandDescriptions
{
	public static class CommandUtil
	{
		public static CommandDescriptionAttribute GetCommandDescription(this Command @enum) {
			string enumItemName = @enum.ToString();
			MemberInfo memInfo = @enum.GetType().GetMember(enumItemName)[0];
			var commandDescriptionAttribute = memInfo.GetCustomAttribute<CommandDescriptionAttribute>();
			if (commandDescriptionAttribute == null) {
				throw new ArgumentException($"{@enum.ToString()} not contains {nameof(CommandDescriptionAttribute)}");
			}
			return commandDescriptionAttribute;
		}

		public static string GetCommandText(this Command @enum) => GetCommandDescription(@enum).Command;

		public static Command[] GetAllCommands()
		{
			return Enum.GetValues(typeof(Command)).Cast<Command>().ToArray();
		}
	}
}