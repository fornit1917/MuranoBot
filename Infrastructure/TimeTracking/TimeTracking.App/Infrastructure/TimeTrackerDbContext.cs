using Microsoft.EntityFrameworkCore;
using MuranoBot.Infrastructure.TimeTracking.App.Models;

namespace MuranoBot.Infrastructure.TimeTracking.App.Infrastructure
{
	public class TimeTrackerDbContext : DbContext, IUnitOfWork
	{
		public DbSet<Vacation> Vacations { get; set; }
		public DbSet<User> Users { get; set; }

		public TimeTrackerDbContext() {}

		public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options)
			: base(options) {}
	}
}