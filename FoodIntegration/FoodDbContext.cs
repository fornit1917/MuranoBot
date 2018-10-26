using Common;
using Microsoft.EntityFrameworkCore;

namespace FoodIntegration
{
	public class FoodDbContext : DbContext
	{
		public DbSet<Menu> Menus { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(AppConfig.Instance.FoodConnectionString);
		}
	}
}