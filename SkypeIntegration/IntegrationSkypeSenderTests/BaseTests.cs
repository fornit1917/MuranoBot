using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntegration.Skype;

namespace IntegrationSkypeSenderTests {
	[TestClass]
	public class BaseTests {
		private SkypeSender _skype;

		[TestInitialize]
		public void StartService() {
			_skype = new SkypeSender("appulateautomaionshared@appulatemail.com", "pwdSkyMurBot");
		}

		[TestCleanup]
		public void ShutDownService() {
			_skype.Dispose();
		}

		[TestMethod]
		public void WriteSomeMessagesToSomeChats() {
			Assert.IsTrue(_skype.Write("Eugene Tikhonov", "Hello 1"));
			Assert.IsTrue(_skype.Write("Public chat", "Hello 2"));
			Assert.IsTrue(_skype.Write("Public chat", "Hello 3"));
			Assert.IsTrue(_skype.Write("Eugene Tikhonov", "Hello 4"));
		}
	}
}
