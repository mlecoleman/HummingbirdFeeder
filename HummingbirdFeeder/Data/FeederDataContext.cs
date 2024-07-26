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

            DateTime today = DateTime.Now;
            string dateString = today.ToString("yyyyMMdd");
            int dateInt = Int32.Parse(dateString);

            modelBuilder.Entity<Feeder>()
                .HasData(
                    new Feeder
                    {
                        FeederId = 1,
                        FeederName = "My Feeder",
                        Zipcode = "40204",
                        LastChangeDate = dateInt,
                        ChangeFeeder = true
                    }
                );
        }
    }
}

