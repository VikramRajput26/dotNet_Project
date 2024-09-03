using Microsoft.EntityFrameworkCore;
using MyProjectJWT.Models;

namespace MyProjectJWT.Context
{
    public class JwtContext : DbContext
    {
        public JwtContext(DbContextOptions<JwtContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Vaccines> Vaccines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define the enum configuration for UserRole
            modelBuilder.Entity<User>()
                .Property(u => u.UserRole)
                .HasConversion<string>();

            // Configuring the relationship between User and Child (One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Children)
                .WithOne()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);  // Deleting a User will delete their Children

            // Configuring the relationship between User and Appointment (One-to-Many)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Appointments)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);  // Deleting a User will delete their Appointments

            // Configuring the relationship between Child and Appointment (One-to-Many)
            modelBuilder.Entity<Child>()
                .HasMany(c => c.Appointments)
                .WithOne(a => a.Child)
                .HasForeignKey(a => a.ChildId)
                .OnDelete(DeleteBehavior.Cascade);  // Deleting a Child will delete their Appointments

            // Configuring the relationship between Appointment and Vaccine (Many-to-One)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Vaccine)
                .WithMany()
                .HasForeignKey(a => a.VaccineId)
                .OnDelete(DeleteBehavior.Restrict); // Deleting an Appointment should not affect the Vaccine

            base.OnModelCreating(modelBuilder);
        }

    }
}
