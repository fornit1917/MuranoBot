using System;
using System.Collections.Generic;
using System.Text;

namespace MuranoBot.TimeTracking.App.Application.Models.Shared {
	public class TimeInterval {
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		public TimeInterval(DateTime start, DateTime end) {
			Start = start;
			End = end;
		}
	}
}
