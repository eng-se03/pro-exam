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

            // العلاقة بين Doctor و Monitoring (1-to-Many)
            modelBuilder.Entity<Doctor>()
                .HasMany(d => d.Monitorings)
                .WithOne(m => m.Doctors)
                .HasForeignKey(m => m.DoctorId)
                .OnDelete(DeleteBehavior.Cascade); // حذف المراقبات عند حذف الطبيب

            // العلاقة بين Schedule و Monitoring (1-to-Many)
            modelBuilder.Entity<Schedule>()
                .HasMany(s => s.Monitorings)
                .WithOne(m => m.Schedule)
                .HasForeignKey(m => m.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade); // حذف المراقبات عند حذف الجدول

            // العلاقة بين Monitoring و Doctor-Schedule (Many-to-Many عبر جدول مركب)
            modelBuilder.Entity<Montering>()
                .HasKey(m => new { m.DoctorId, m.ScheduleId }); // تعريف المفتاح المركب

        }

        public DbSet <Doctor> Doctors { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Montering> Montering { get; set; }
        public DbSet<Exam> Exams { get; set; }



    }
}

