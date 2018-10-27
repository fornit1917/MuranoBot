using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuranoBot.Infrastructure.TimeTracking.App.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure;

namespace MuranoBot.Infrastructure.TimeTracking.App.Infrastructure.Repositories {
	public class VacationsRepository {
		private readonly TimeTrackerDbContext _dbContext;

		public VacationsRepository(TimeTrackerDbContext dbContext) {
			_dbContext = dbContext;
		}

		public IUnitOfWork UnitOfWork => _dbContext;

		public Vacation Get(string userName, DateTime at) {
			return _dbContext.Vacations
				.Where(x => x.User.UserName == userName && x.DateFrom <= at && at <= x.DateTo)
				.OrderByDescending(x => x.Id)
				.FirstOrDefault();
		}

		public Vacation Add(Vacation vacation) {
			return _dbContext.Vacations.Add(vacation).Entity;
		}
	}
}
