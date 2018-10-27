using OpenQA.Selenium;

namespace SkypeIntegration.SeleniumCore.Elements {
	public class InputField : WebElementBase {
		public InputField(IWebElement webElement) : base(webElement) { }

		public void Fill(string value) {
			WebElement.Clear();
			WebElement.SendKeys(value);
		}
	}
}