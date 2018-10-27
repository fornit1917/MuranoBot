using System;
using System.IO;
using Common;
using SkypeIntegration.SeleniumCore;
using SkypeIntegration.SeleniumCore.Elements;
using SkypeIntegration.Skype.Bys;

namespace SkypeIntegration.Skype {
	public class SkypeSender : IDisposable {
		public SkypeSender(string login, string password) {
			SkypeWebVersionUri = new Uri("https://web.skype.com/en/");
			Login = login;
			Password = password;
			TryCountOnError = 2;
			InitBrowser();
		}

		public SkypeSender() : this(AppConfig.Instance.SkypeLogin, AppConfig.Instance.SkypePassword) { }

		private ChromeBrowser Browser { get; set; }

		private Uri SkypeWebVersionUri { get; }
		private string Login { get; }
		private string Password { get; }
		private int TryCountOnError { get; }

		public bool Write(string chatName, string message) {
			for (var i = 0; i < TryCountOnError; i++) {
				try {
					OpenChat(chatName);
					SendMessage(message);
					return true;
				} catch (Exception e) {
					Console.WriteLine($"Error while sending message '{message}' to chat '{chatName}': {e}");
					InitBrowser();
				}
			}
			return false;
		}

		private void LoginToSite() {
			Browser.Navigate(SkypeWebVersionUri);
			Browser.Element<InputField>(LoginPageBys.EmailField)
				   .Fill(Login);
			Browser.Element<Button>(LoginPageBys.SubmitButton)
				   .Click();

			Browser.Element<InputField>(LoginPageBys.PasswordField)
				   .Fill(Password);
			Browser.Element<Button>(LoginPageBys.SubmitButton)
				   .Click();

			Browser.Wait.ForElement(MainPageBys.MyInfoIcon, TimeSpan.FromMinutes(1));
		}

		private void OpenChat(string chatName) {
			Browser.Element<Button>(MainPageBys.SwitchToChatButton(chatName))
				   .Click();
			Browser.Wait.ForElement(MainPageBys.ExpectedActiveChatIcon(chatName));
		}

		private void SendMessage(string message) {
			Browser.Element<Button>(MainPageBys.MessageInputContainer, TimeSpan.FromSeconds(50))
				   .Click();
			Browser.Element<InputField>(MainPageBys.MessageInput)
				   .Fill(message);
			Browser.Element<Button>(MainPageBys.MessageSendButton)
				   .Click();
		}

		private void InitBrowser() {
			try {
				var a = Directory.GetCurrentDirectory();
				Browser?.Dispose();
				Browser = ChromeBrowser.OpenBrowser();
				LoginToSite();
			} catch (Exception e) {
				Console.WriteLine($"Exception when initializing: {e}");
			}
		}

		public void Dispose() {
			Browser?.Dispose();
		}
	}
}