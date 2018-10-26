using Common;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
	public class DomainDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<MessengerLink> MessengerLinks { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(AppConfig.Instance.MainConnectionString);
		}
	}
}