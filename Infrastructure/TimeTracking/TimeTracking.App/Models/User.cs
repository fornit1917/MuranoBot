using System.ComponentModel.DataAnnotations.Schema;

namespace MuranoBot.Infrastructure.TimeTracking.App.Models {
	[Table("TT_BaseUsers")]
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
