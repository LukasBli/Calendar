using Microsoft.EntityFrameworkCore;
using WeeklyPlanner.Core.Models;

namespace WeeklyPlanner.Data.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().ToTable(nameof(Appointments));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("TestDB");
            }
        }

        public DbSet<Appointment> Appointments { get; set; }
    }
}
