﻿using System;
using System.Collections.Generic;
using System.Text;
using MuranoBot.TimeTracking.App.Application.Models.Shared;

namespace MuranoBot.TimeTracking.App.Application.Models {
	public class VacationInfo {
		public int UserId { get; set; }
		public TimeInterval Interval { get; set; }
	}
}