using System;
using Microsoft.EntityFrameworkCore;

namespace HummingbirdFeeder.Data
{
	public class FeederDataContext : DbContext
	{
        protected readonly IConfiguration Configuration;

        public FeederDataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("FeederDB"));
        }

        public DbSet<Feeder> Feeders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feeder>()
                .ToTable("Feeder");

            modelBuilder.Entity<Feeder>()
                .HasData(
                    new Feeder
                    {
                        FeederId = 1,
                        Zipcode = 40204
                    }
                );
        }
    }
}

