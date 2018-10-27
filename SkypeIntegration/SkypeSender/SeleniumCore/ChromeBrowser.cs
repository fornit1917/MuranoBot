using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SkypeIntegration.SeleniumCore.Elements;

namespace SkypeIntegration.SeleniumCore {
	public class ChromeBrowser : IDisposable {
		private ChromeBrowser(ChromeDriverService service, ChromeDriver webDriver) {
			ChromeService = service;
			Browser = webDriver;
			Wait = new Waiter(this);
		}

		public static ChromeBrowser OpenBrowser() {
			var options = new ChromeOptions();
			//options.AddArguments("--headless");
			options.AddArguments("--disable-gpu");
			options.AddArguments("--disable-notifications");
			var chromeService = ChromeDriverService.CreateDefaultService();
			chromeService.HideCommandPromptWindow = true;
			chromeService.EnableVerboseLogging = false;

			ChromeDriver webDriver = new ChromeDriver(chromeService, options);
			webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(250);

			return new ChromeBrowser(chromeService, webDriver);
		}

		private IWebDriver Browser { get; }
		private ChromeDriverService ChromeService { get; }

		public void Navigate(Uri uri) {
			Browser.Navigate().GoToUrl(uri);
		}

		public T Element<T>(By by, TimeSpan? timeout = null) where T : WebElementBase => Wait.ForElement<T>(by, timeout);

		public T ElementImmediately<T>(By by) where T : WebElementBase {
			var webElement = Browser.FindElements(by).FirstOrDefault();
			try {
				if (webElement == null || !webElement.Displayed) {
					return null;
				}

				var element = (T)Activator.CreateInstance(typeof(T), webElement);
				return element.Visible() ? element : null;
			} catch (StaleElementReferenceException) {
				return null;
			}
		}

		public Waiter Wait { get; }

		public void Dispose() {
			Browser?.Quit();
			ChromeService?.Dispose();
		}

		public class Waiter {
			private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

			public Waiter(ChromeBrowser browser) {
				Browser = browser;
			}

			private ChromeBrowser Browser { get; }

			public void ForElement(By by, TimeSpan? timeout = null) => ForElement<ReadonlyText>(by, timeout);

			public T ForElement<T>(By by, TimeSpan? timeout = null, bool requireVisible = true) where T : WebElementBase {
				var timeoutForFind = timeout ?? DefaultTimeout;

				var stopWatch = new Stopwatch();
				stopWatch.Start();

				while (stopWatch.Elapsed < timeoutForFind) {
					var webElement = Browser.ElementImmediately<T>(by);
					if (webElement != null) {
						stopWatch.Stop();
						return webElement;
					}
					Thread.Sleep(100);
				}

				throw new TimeoutException($"Waiting {timeoutForFind.TotalSeconds} seconds for element {by}");
			}
		}
	}
}