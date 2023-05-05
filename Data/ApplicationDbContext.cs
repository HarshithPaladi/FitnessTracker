using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FitnessTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<FitnessUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WorkoutsModel> Workouts { get; set; }
        public DbSet<FitnessUser> FitnessUsers { get; set; }
        public DbSet<VitalsModel> Vitals { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Workout>()
            //    .HasOne(w => w.User)
            //    .WithMany(u => u.Workouts)
            //    .HasForeignKey(w => w.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}