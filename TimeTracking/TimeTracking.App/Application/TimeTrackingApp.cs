using System;
using MuranoBot.TimeTracking.App.Application.Models;
using MuranoBot.TimeTracking.App.Application.Models.Shared;
using MuranoBot.TimeTracking.App.Infrastructure.Repositories;

namespace MuranoBot.TimeTracking.App.Application {
	public class TimeTrackingApp {
		private readonly VacationsRepository _vacationsRepository;
		public TimeTrackingApp(VacationsRepository vacationsRepository) {
			_vacationsRepository = vacationsRepository;
		}

		public VacationInfo GetVacationInfo(int userId, DateTime at) {
			var vacation = _vacationsRepository.Get(userId, at);
			return new VacationInfo {
				Interval = new TimeInterval(vacation.DateFrom, vacation.DateTo)
			};
		}

		public void SetVacation(VacationInfo info) {

		}
	}
}
