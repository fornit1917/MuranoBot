using Microsoft.EntityFrameworkCore;
using MuranoBot.Infrastructure.TimeTracking.App.Infrastructure;

namespace MuranoBot.Infrastructure.TimeTracking.Tests.Application {
	public abstract class TestBase {
		protected TimeTrackerDbContext CreateTestContext() {
			var optionsBuilder = new DbContextOptionsBuilder<TimeTrackerDbContext>();
			optionsBuilder.UseSqlServer("Server=localhost;Database=TimeTrackerNew;Trusted_Connection=True;");
			return new TimeTrackerDbContext(optionsBuilder.Options);
		}
	}
}