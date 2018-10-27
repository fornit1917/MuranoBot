using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuranoBot.Infrastructure.TimeTracking.App.Application;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models.Shared;
using MuranoBot.Infrastructure.TimeTracking.App.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.Infrastructure.TimeTracking.Tests.Application {
	[TestClass]
	public class UsersAppTest : TestBase {
		[TestMethod]
		public void GetUserInfoTest() {
			using(new TransactionScope(TransactionScopeOption.RequiresNew))
			using (var dbContext = new TimeTrackerDbContext()) {
				// Arrange
				string domainName = @"CORP\Maxim.Rozhkov";
				int userId = 1091;
				string email = "maxim.rozhkov@muranosoft.com";
				var app = new UsersApp(new UsersRepository(dbContext));

				// Act
				var rc = app.GetUserInfo(domainName);

				//Assert
				Assert.AreEqual(userId, rc.Id);
				Assert.AreEqual(domainName, rc.DomainDame);
				Assert.AreEqual(email, rc.Email);
			}
		}
	}
}
