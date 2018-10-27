using OpenQA.Selenium;

namespace SkypeIntegration.SeleniumCore.Elements {
	public class ReadonlyText : WebElementBase {
		public ReadonlyText(IWebElement webElement) : base(webElement) { }

		public string Text => WebElement.Text;
	}
}