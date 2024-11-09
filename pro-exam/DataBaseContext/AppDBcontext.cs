using Microsoft.EntityFrameworkCore;
using pro_exam.Models;

namespace pro_exam.DataBaseContext
{
    public class AppDBcontext : DbContext
    {
        public AppDBcontext(DbContextOptions options) : base(options)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {

            modelBuilder.Entity<Doctor>()
           .HasIndex(u => u.DoctorName)
           .IsUnique();


        }

        public DbSet <Doctor> Doctors { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Montering> Montering { get; set; }


    }
}

