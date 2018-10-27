using Microsoft.EntityFrameworkCore;

namespace FoodIntegration
{
	public class FoodDbContext : DbContext
	{
		public DbSet<Menu> Menus { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<User> Users { get; set; }

		public FoodDbContext(DbContextOptions<FoodDbContext> options)
			: base(options) { }
	}
}