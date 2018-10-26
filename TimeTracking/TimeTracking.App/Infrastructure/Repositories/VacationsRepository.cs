﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuranoBot.TimeTracking.App.Models;
using MuranoBot.TimeTracking.App.Infrastructure;

namespace MuranoBot.TimeTracking.App.Infrastructure.Repositories {
	public class VacationsRepository {
		private readonly TimeTrackerDbContext _dbContext;

		public VacationsRepository(TimeTrackerDbContext dbContext) {
			_dbContext = dbContext;
		}

		public IUnitOfWork UnitOfWork => _dbContext;

		public Vacation Get(int userId, DateTime at) {
			return _dbContext.Vacations
				.Where(x => x.UserId == userId && x.DateFrom <= at && at <= x.DateTo)
				.FirstOrDefault();
		}

		public Vacation Add(Vacation vacation) {
			return _dbContext.Vacations.Add(vacation).Entity;
		}
	}
}
