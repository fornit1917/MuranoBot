﻿using System;
using System.Collections.Generic;
using System.Text;
using MuranoBot.Common;

namespace MuranoBot.Infrastructure.TimeTracking.Tests.Application {
	public class TestBase {
		public TestBase() {
			var appConfig = AppConfig.Instance;
			appConfig.TimeTrackerConnectionString = @"Server=localhost;Database=TimeTrackerNew;Trusted_Connection=True;";
		}
	}
}
