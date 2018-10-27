using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MuranoBot.Infrastructure.TimeTracking.App.Application;

namespace MuranoBot.Application.Commands {
	public class CheckVacationCommandHandler : IRequestHandler<CheckVacationCommand, bool> {
		private readonly VacationsApp _vacationsApp;
		public CheckVacationCommandHandler(VacationsApp vacationsApp) {
			_vacationsApp = vacationsApp;
		}

		public Task<bool> Handle(CheckVacationCommand request, CancellationToken cancellationToken) {
			var rc = _vacationsApp.GetVacationInfo(1091, new DateTime(2018, 08, 27));
			return Task.FromResult(true);
		}
	}
}
