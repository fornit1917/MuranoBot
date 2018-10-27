using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace SkypeIntegration.Skype.Bys {
	internal class MainPageBys {
		public static By MyInfoIcon = By.ClassName("Me-info");
		public static By SwitchToChatButton(string chatName) => By.CssSelector($"span.topic[title='{chatName}']");
		public static By ExpectedActiveChatIcon(string chatName) => new ByAll(By.CssSelector("[data-swx-testid='conversationTopic']"), By.XPath($".//*[text() = '{chatName}']"));
		public static By MessageInput = By.CssSelector("#chatInputAreaWithQuotes");
		public static By MessageInputContainer = By.Id("chatInputContainer");
		public static By MessageSendButton = By.CssSelector("button.swx-chat-input-send-btn");
	}
}