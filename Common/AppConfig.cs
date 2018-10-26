namespace Common
{
	public class AppConfig
	{
		public static readonly AppConfig Instance = new AppConfig();

		public bool RunSlackBot { get; set; }
		public bool RunSkypeBot { get; set; }
		public bool RunTelegramBot { get; set; }
        public bool MockSender { get; set; }
		public string SlackToken { get; set; }
		public string FoodConnectionString { get; set; }
		public string MainConnectionString { get; set; }
	}
}