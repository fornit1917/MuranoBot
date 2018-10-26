using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuranoBot.TimeTracking.App.Application;
using MuranoBot.TimeTracking.App.Models;
using MuranoBot.TimeTracking.App.Infrastructure;
using MuranoBot.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.TimeTracking.Tests {
	[TestClass]
	public class TimeTrackingAppTest {

		[TestMethod]
		public void GetVacationInfoTest() {
			using(new TransactionScope(TransactionScopeOption.RequiresNew))
			using (var dbContext = new TimeTrackerDbContext()) {
				// Arrange
				int userId = 1091;
				DateTime from = new DateTime(2018, 08, 24);
				DateTime to = new DateTime(2018, 09, 12);
				dbContext.Vacations.Add(new Vacation(userId, from, to));
				dbContext.SaveChanges();
				var app = new TimeTrackingApp(new VacationsRepository(dbContext));

				// Act
				var rc = app.GetVacationInfo(userId, new DateTime(2018, 08, 27));

				//Assert
				Assert.AreEqual(rc.Interval.Start, from);
				Assert.AreEqual(rc.Interval.End, to);
			}
		}
	}
}
