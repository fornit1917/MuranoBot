using OpenQA.Selenium;

namespace SkypeIntegration.SeleniumCore.Elements {
	public class Button : WebElementBase {
		public Button(IWebElement webElement) : base(webElement) { }

		public void Click() {
			WebElement.Click();
		}
	}
}