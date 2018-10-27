using System;
using System.Threading;
using MuranoBot.Common;
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

		public SkypeSender(AppConfig appConfig) : this(appConfig.SkypeLogin, appConfig.SkypePassword) { }

		private ChromeBrowser Browser { get; set; }

		private Uri SkypeWebVersionUri { get; }
		private string Login { get; }
		private string Password { get; }
		private int TryCountOnError { get; }

		private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);
		private readonly ConcurrentGroupQueue _queueForSend = new ConcurrentGroupQueue();

		public bool Write(string chatName, string message) {
			_queueForSend.Add(chatName, message);

			_semaphore.Wait();
			var valueForSend = _queueForSend.GetNext();
			if (valueForSend == null) {
				_semaphore.Release();
				Console.WriteLine($"Empty queue on write message '{message}' to chat '{chatName}'");
				return false;
			}

			var chatNameForSend = valueForSend.Value.Key;
			var messageForSend = valueForSend.Value.Value;

			for (var i = 0; i < TryCountOnError; i++) {
				try {
					OpenChat(chatNameForSend);
					SendMessage(messageForSend);
					_semaphore.Release();
					return true;
				} catch (Exception e) {
					Console.WriteLine($"Error while sending message '{messageForSend}' to chat '{chatNameForSend}': {e}");
					InitBrowser();
				}
			}
			_semaphore.Release();
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