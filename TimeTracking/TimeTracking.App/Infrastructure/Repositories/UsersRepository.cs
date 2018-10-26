using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuranoBot.TimeTracking.App.Models;
using MuranoBot.TimeTracking.App.Infrastructure;

namespace MuranoBot.TimeTracking.App.Infrastructure.Repositories {
	public class UsersRepository {
		private readonly TimeTrackerDbContext _dbContext;

		public UsersRepository(TimeTrackerDbContext dbContext) {
			_dbContext = dbContext;
		}

		public User Get(string userName) {
			return _dbContext.Users
				.Where(x => x.UserName == userName)
				.FirstOrDefault();
		}
	}
}
