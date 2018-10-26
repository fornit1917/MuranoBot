using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MuranoBot.TimeTracking.App.Application;
using MuranoBot.TimeTracking.App.Application.Models;
using MuranoBot.TimeTracking.App.Application.Models.Shared;
using MuranoBot.TimeTracking.App.Models;
using MuranoBot.TimeTracking.App.Infrastructure;
using MuranoBot.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.TimeTracking.Tests.Application {
	[TestClass]
	public class UsersAppTest : TestBase {
		[TestMethod]
		public void GetVacationInfoTest() {
			using(new TransactionScope(TransactionScopeOption.RequiresNew))
			using (var dbContext = new TimeTrackerDbContext()) {
				// Arrange

				// Act

				//Assert
			}
		}
	}
}
