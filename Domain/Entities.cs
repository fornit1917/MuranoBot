using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuranoBot.Domain
{
	[Table(nameof(User))]
	public class User
	{
		[Key]
		public int Id { get; set; }
		[Column(TypeName = "varchar(256)")]
		public string Email { get; set; }
	}

	[Table(nameof(MessengerLink))]
	public class MessengerLink
	{
		public int? UserId { get; set; }
		[Column(TypeName = "tinyint")]
		public Messenger Messenger { get; set; }
		[Column(TypeName = "varchar(256)")]
		public string ExternalUserId { get; set; }
		public Guid AuthToken { get; set; }

		[ForeignKey(nameof(UserId))]
		public virtual User User { get; set; }
	}
}