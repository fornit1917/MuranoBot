using OpenQA.Selenium;

namespace SkypeIntegration.Skype.Bys {
	internal class LoginPageBys {
		public static By EmailField = InputByType("email");
		public static By PasswordField = InputByType("password");
		public static By SubmitButton = InputByType("submit");

		private static By InputByType(string type) => By.CssSelector($"input[type = '{type}']");
	}
}