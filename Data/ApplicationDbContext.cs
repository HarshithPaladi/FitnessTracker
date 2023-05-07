using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
// using Newtonsoft.Json;
using System.Text.Json;

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
            //builder.Entity<WorkoutsModel>()
            //    .HasOne(w => w.Vitals)
            //    .WithMany()
            //    .HasForeignKey(w => w.VitalsId)
            //    .IsRequired(false);

            //builder.Entity<VitalsModel>()
            //    .Property(v => v.Date)
            //    .IsRequired(false);
            //builder.Entity<VitalsModel>()
            //    .Property(v => v.FitnessUserId)
            //    .IsRequired(false);

            //builder.Entity<Workout>()
            //    .HasOne(w => w.User)
            //    .WithMany(u => u.Workouts)
            //    .HasForeignKey(w => w.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);
        }

    }
}