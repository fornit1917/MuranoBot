using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
	[Table(nameof(User))]
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; }
	}

	[Table(nameof(MessengerLink))]
	public class MessengerLink
	{
		public int? UserId { get; set; }
		public Messenger MessengerId { get; set; }
		public string ExternalUserId { get; set; }
		public string AuthHash { get; set; }
	}
}