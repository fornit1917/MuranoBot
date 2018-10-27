using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntegration.Skype;

namespace IntegrationSkypeSenderTests {
	[TestClass]
	public class BaseTests {
		private static SkypeSender _skype;
		private const string SkypeLogin = "";
		private const string SkypePassword = "";

		private const string PrivateChatTitle = "Eugene Tikhonov";
		private const string PublicChatTitle = "Public chat";

		[ClassInitialize]
		public static void StartService(TestContext context) {
			_skype = new SkypeSender(SkypeLogin, SkypePassword);
		}

		[ClassCleanup]
		public static void ShutDownService() {
			_skype.Dispose();
		}

		[TestMethod]
		public void WriteSomeMessagesToSomeChats() {
			Assert.IsTrue(_skype.Write(PrivateChatTitle, "Hello 1"), "Error while sending first private message");
			Assert.IsTrue(_skype.Write(PublicChatTitle, "Hello 2"), "Error while sending first public chat message");
			Assert.IsTrue(_skype.Write(PublicChatTitle, "Hello 3"), "Error while sending second public chat message");
			Assert.IsTrue(_skype.Write(PrivateChatTitle, "Hello 4"), "Error while sending second private message");
		}

		[TestMethod]
		public void AsyncWriteSomeMessagesToSomeChats() {
			var to = new[] {
				PrivateChatTitle,
				PublicChatTitle
			};

			var tasks = new List<Task<bool>>();
			var funcs = new List<Func<bool>>();

			for (var i = 0; i < 10; i++) {
				var copyI = i;
				funcs.Add(() => _skype.Write(to[copyI % to.Length], $"async Hello #{copyI}"));
			}

			funcs.ForEach(f => tasks.Add(Task.Run(f)));

			foreach (var task in tasks) {
				Assert.IsTrue(task.Result);
			}
		}
	}
}
