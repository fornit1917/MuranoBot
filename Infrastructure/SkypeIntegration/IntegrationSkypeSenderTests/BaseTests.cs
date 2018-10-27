using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkypeIntegration.Skype;

namespace IntegrationSkypeSenderTests {
	[TestClass]
	public class BaseTests {
		private static SkypeSender _skype;

		[ClassInitialize]
		public static void StartService(TestContext context) {
			_skype = new SkypeSender("appulateautomaionshared@appulatemail.com", "pwdSkyMurBot");
		}

		[ClassCleanup]
		public static void ShutDownService() {
			_skype.Dispose();
		}

		[TestMethod]
		public void WriteSomeMessagesToSomeChats() {
			Assert.IsTrue(_skype.Write("Eugene Tikhonov", "Hello 1"));
			Assert.IsTrue(_skype.Write("Public chat", "Hello 2"));
			Assert.IsTrue(_skype.Write("Public chat", "Hello 3"));
			Assert.IsTrue(_skype.Write("Eugene Tikhonov", "Hello 4"));
		}

		[TestMethod]
		public void AsyncWriteSomeMessagesToSomeChats() {
			var to = new [] {
				"Eugene Tikhonov",
				"Public chat" 
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
