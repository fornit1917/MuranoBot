using System;
using System.Collections.Generic;
using System.Text;
using MuranoBot.Infrastructure.TimeTracking.App.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.Infrastructure.TimeTracking.App.Application {
	public class UsersApp {
		private readonly UsersRepository _usersRepository;

		public UsersApp(UsersRepository usersRepository) {
			_usersRepository = usersRepository;
		}

		public UserInfo GetUserInfo(string domainName) {
			var user = _usersRepository.Get(userName: domainName);

			return user != null ? new UserInfo { DomainDame = user.UserName, Id = user.UserId, Email = user.Email } : null;
		}
	}
}
