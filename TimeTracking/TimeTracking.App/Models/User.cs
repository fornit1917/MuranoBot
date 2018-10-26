using System;
using System.Collections.Generic;
using System.Text;

namespace MuranoBot.TimeTracking.App.Models {
	public class User {

		public int UserId { get; protected set; }
		public string UserName { get; protected set; }
		public string Email { get; protected set; }

		public User() { }
		public User(int userId, string userName, string email) {
			UserId = userId;
			UserName = userName;
			Email = email;
		}
	}
}
