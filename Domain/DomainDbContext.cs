using MuranoBot.Common;
using Microsoft.EntityFrameworkCore;

namespace MuranoBot.Domain
{
	public class DomainDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<MessengerLink> MessengerLinks { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(AppConfig.Instance.MainConnectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<MessengerLink>().HasKey(e => new { e.Messenger, e.ExternalUserId });
		}
	}
}