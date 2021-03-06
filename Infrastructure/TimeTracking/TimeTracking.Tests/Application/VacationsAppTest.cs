using System;
using System.Transactions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuranoBot.Infrastructure.TimeTracking.App.Application;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models.Shared;
using MuranoBot.Infrastructure.TimeTracking.App.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.Infrastructure.TimeTracking.Tests.Application {
	[TestClass]
	public class VacationsAppTest : TestBase {
		[TestMethod]
		public void GetVacationInfoTest() {
			using(new TransactionScope(TransactionScopeOption.RequiresNew))
			using (TimeTrackerDbContext dbContext = CreateTestContext()) {
				// Arrange
				int userId = 1091;
				string domainName = @"CORP\Maxim.Rozhkov";
				DateTime from = new DateTime(2018, 08, 24);
				DateTime to = new DateTime(2018, 09, 12);
				dbContext.Vacations.Add(new Vacation(userId, from, to));
				dbContext.SaveChanges();
				var app = new VacationsApp(new UsersRepository(dbContext), new VacationsRepository(dbContext));

				// Act
				var rc = app.GetVacationInfo(domainName, new DateTime(2018, 08, 27));

				//Assert
				Assert.AreEqual(rc.Interval.Start, from);
				Assert.AreEqual(rc.Interval.End, to);
			}
		}

		[TestMethod]
		public void SetVacationTest() {
			using(new TransactionScope(TransactionScopeOption.RequiresNew))
			using (TimeTrackerDbContext dbContext = CreateTestContext()) {
				// Arrange
				string domainName = @"CORP\Maxim.Rozhkov";
				DateTime from = new DateTime(2018, 08, 24);
				DateTime to = new DateTime(2018, 09, 12);
				var app = new VacationsApp(new UsersRepository(dbContext), new VacationsRepository(dbContext));

				// Act
				app.SetVacation(new VacationInfo {
					DomainName = domainName,
					Interval = new TimeInterval(from, to),
				});

				//Assert
				var vacation = dbContext.Vacations
					.Where(x => x.User.UserName == domainName && x.DateFrom == from && x.DateTo == to);
				Assert.IsNotNull(vacation);
			}
		}
	}
}