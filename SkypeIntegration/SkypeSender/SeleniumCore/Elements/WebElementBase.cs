using OpenQA.Selenium;

namespace SkypeIntegration.SeleniumCore.Elements {
	public abstract class WebElementBase {
		public IWebElement WebElement { get; }

		protected WebElementBase(IWebElement webElement) {
			WebElement = webElement;
		}

		public bool Visible() => WebElement.Displayed;
	}
}