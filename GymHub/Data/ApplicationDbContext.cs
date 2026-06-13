using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GymHub.Models;

namespace GymHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Membership> Memberships { get; set; }
        public DbSet<GymClass> GymClasses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<FitnessContent> FitnessContents { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Membership>()
                .Property(m => m.Price)
                .HasPrecision(18, 2);
            { }
            modelBuilder.Entity<Membership>().HasData(
                new Membership
                {
                    MembershipId = 1,
                    MemberName = "John Smith",
                    MembershipType = "Basic",
                    Price = 19.99m,
                    Duration = 30,
                    StartDate = new DateTime(2026, 1, 1),
                    Status = "Active"
                },
                new Membership
                {
                    MembershipId = 2,
                    MemberName = "Sarah Johnson",
                    MembershipType = "Premium",
                    Price = 34.99m,
                    Duration = 30,
                    StartDate = new DateTime(2026, 1, 5),
                    Status = "Active"
                },
                new Membership
                {
                    MembershipId = 3,
                    MemberName = "Michael Brown",
                    MembershipType = "VIP",
                    Price = 49.99m,
                    Duration = 30,
                    StartDate = new DateTime(2026, 1, 10),
                    Status = "Active"
                }
            );

            modelBuilder.Entity<GymClass>().HasData(
                new GymClass
                {
                    GymClassId = 1,
                    ClassName = "Yoga",
                    ClassDate = new DateTime(2026, 1, 15),
                    StartTime = "09:00",
                    EndTime = "10:00",
                    Instructor = "Sarah Ahmed",
                    DifficultyLevel = "Beginner",
                    Capacity = 20
                },
                new GymClass
                {
                    GymClassId = 2,
                    ClassName = "HIIT",
                    ClassDate = new DateTime(2026, 1, 16),
                    StartTime = "17:00",
                    EndTime = "18:00",
                    Instructor = "James Wilson",
                    DifficultyLevel = "Advanced",
                    Capacity = 15
                },
                new GymClass
                {
                    GymClassId = 3,
                    ClassName = "Strength Training",
                    ClassDate = new DateTime(2026, 1, 17),
                    StartTime = "18:00",
                    EndTime = "19:00",
                    Instructor = "Michael Brown",
                    DifficultyLevel = "Intermediate",
                    Capacity = 25
                }
            );

            modelBuilder.Entity<FitnessContent>().HasData(
                new FitnessContent
                {
                    FitnessContentId = 1,
                    Title = "Beginner Workout Plan",
                    Description = "A workout plan designed for new gym members.",
                    ImagePath = null
                }
            );
        }
    }
}