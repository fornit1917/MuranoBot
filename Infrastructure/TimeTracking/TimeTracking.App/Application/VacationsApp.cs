using System;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models.Shared;
using MuranoBot.Infrastructure.TimeTracking.App.Models;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.Infrastructure.TimeTracking.App.Application {
	public class VacationsApp {
		private readonly VacationsRepository _vacationsRepository;
		public VacationsApp(VacationsRepository vacationsRepository) {
			_vacationsRepository = vacationsRepository;
		}

		public VacationInfo GetVacationInfo(int userId, DateTime at) {
			var vacation = _vacationsRepository.Get(userId, at);
			return new VacationInfo {
				Interval = new TimeInterval(vacation.DateFrom, vacation.DateTo)
			};
		}

		public void SetVacation(VacationInfo info) {
			var vacation = new Vacation(info.UserId, info.Interval.Start, info.Interval.End);
			_vacationsRepository.Add(vacation);
			_vacationsRepository.UnitOfWork.SaveChangesAsync();
		}
	}
}
