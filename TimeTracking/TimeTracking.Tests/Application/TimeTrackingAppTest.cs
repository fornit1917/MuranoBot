using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuranoBot.TimeTracking.App.Application;
using MuranoBot.TimeTracking.App.Application.Models;
using MuranoBot.TimeTracking.App.Application.Models.Shared;

namespace MuranoBot.TimeTracking.Tests {
	[TestClass]
	public class TimeTrackingAppTest {
		[TestMethod]
		public void GetVacationInfoTest() {
			// Arrange
			var app = new TimeTrackingApp();

			// Act
			var rc = app.GetVacationInfo(new DateTime(2018, 08, 27));

			//Assert
			Assert.AreEqual(rc, new VacationInfo {
				Interval = new TimeInterval(new DateTime(2018, 08, 24), new DateTime(2018, 09, 12))
			});
		}
	}
}
