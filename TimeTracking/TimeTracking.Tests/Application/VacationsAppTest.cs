using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using MuranoBot.TimeTracking.App.Application;
using MuranoBot.TimeTracking.App.Application.Models;
using MuranoBot.TimeTracking.App.Application.Models.Shared;
using MuranoBot.TimeTracking.App.Models;
using MuranoBot.TimeTracking.App.Infrastructure;
using MuranoBot.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.TimeTracking.Tests.Application {
	[TestClass]
	public class VacationsAppTest : TestBase {
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
				var app = new VacationsApp(new VacationsRepository(dbContext));

				// Act
				var rc = app.GetVacationInfo(userId, new DateTime(2018, 08, 27));

				//Assert
				Assert.AreEqual(rc.Interval.Start, from);
				Assert.AreEqual(rc.Interval.End, to);
			}
		}

		[TestMethod]
		public void SetVacationTest() {
			using(new TransactionScope(TransactionScopeOption.RequiresNew))
			using (var dbContext = new TimeTrackerDbContext()) {
				// Arrange
				int userId = 1091;
				DateTime from = new DateTime(2018, 08, 24);
				DateTime to = new DateTime(2018, 09, 12);
				var app = new VacationsApp(new VacationsRepository(dbContext));

				// Act
				app.SetVacation(new VacationInfo {
					UserId = userId,
					Interval = new TimeInterval(from, to),
				});

				//Assert
				var vacation = dbContext.Vacations
					.Where(x => x.UserId == userId && x.DateFrom == from && x.DateTo == to);
				Assert.IsNotNull(vacation);
			}
		}
	}
}
