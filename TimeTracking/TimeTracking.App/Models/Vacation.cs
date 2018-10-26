using System;
using System.Collections.Generic;
using System.Text;

namespace MuranoBot.TimeTracking.App.Models {
	public class Vacation {
		public int Id { get; protected set; }
		public int UserId { get; protected set; }

		public DateTime DateFrom { get; protected set; }
		public DateTime DateTo { get; protected set; }

		public Vacation() { }
		public Vacation(int userId, DateTime from, DateTime to) {
			UserId = userId;
			DateFrom = from;
			DateTo = to;
		}
	}
}
