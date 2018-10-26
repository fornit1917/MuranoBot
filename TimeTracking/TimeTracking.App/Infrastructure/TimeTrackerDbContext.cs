using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Common;
using MuranoBot.TimeTracking.App.Models;

namespace MuranoBot.TimeTracking.App.Infrastructure {
	public class TimeTrackerDbContext : DbContext, IUnitOfWork {

		public DbSet<Vacation> Vacations { get; set; }

		public TimeTrackerDbContext() { }

		public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options)
			:base(options) {

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) { 
		}
		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			optionsBuilder.UseSqlServer(AppConfig.Instance.TimeTrackerConnectionString);
		}
	}
}
