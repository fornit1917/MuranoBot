using System;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models.Shared;
using MuranoBot.Infrastructure.TimeTracking.App.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.Infrastructure.TimeTracking.App.Application {
	public class VacationsApp {
		private readonly UsersRepository _usersRepository;
		private readonly VacationsRepository _vacationsRepository;

		public VacationsApp(UsersRepository usersRepository, VacationsRepository vacationsRepository) {
			_usersRepository = usersRepository;
			_vacationsRepository = vacationsRepository;
		}

		public VacationInfo GetVacationInfo(string domainName, DateTime at) {
			var vacation = _vacationsRepository.Get(domainName, at);
			return new VacationInfo {
				Interval = new TimeInterval(vacation.DateFrom, vacation.DateTo)
			};
		}

		public void SetVacation(VacationInfo info) {
			var user = _usersRepository.Get(info.DomainName);
			var vacation = new Vacation(user.UserId, info.Interval.Start, info.Interval.End);
			_vacationsRepository.Add(vacation);
			_vacationsRepository.UnitOfWork.SaveChanges();
		}
	}
}
