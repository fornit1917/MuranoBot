﻿namespace MuranoBot.Common 
{
	public class AppConfig
	{
		public bool RunSlackBot { get; set; }
		public bool RunSkypeBot { get; set; }
		public bool RunTelegramBot { get; set; }
        public bool MockSender { get; set; }
		public string SlackToken { get; set; }
		public string SkypeLogin { get; set; }
		public string SkypePassword { get; set; }
		public string FoodConnectionString { get; set; }
		public string TimeTrackerConnectionString { get; set; }
		public string MainConnectionString { get; set; }
		public string FoodMenuLink { get; set; }
        public string SkypeOfficeChat { get; set; }
        public string TelegramToken { get; set; }
        public string TelegramProxyHost { get; set; }
        public int TelegramProxyPort { get; set; }
		public string SlackOfficeChat { get; set; }
		public string BindAccountUrl { get; set; }
		public string NewMenuCheckJobSchedule { get; set; }
		public string OrderIsMadeJobSchedule { get; set; }
	}
}
