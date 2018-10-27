namespace MuranoBot.Application.Commands.CommandDescriptions
{
	public enum Command
	{
		[CommandDescription(Command = "скажи всем:", Hint = "сделать оповещение в общие каналы Slack, Skype, Telegram")]
		Repost,
		[CommandDescription(Command = "в отпуске", Hint = "узнать в отпуске ли ваш коллега")]
		CheckVacation,
		[CommandDescription(Command = "ухожу в отпуск с <date> по <date>", Hint = "занести даты вашего отпуска в TT")]
		SetVacation
	}
} 