using Microsoft.AspNetCore.Identity;
//using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace FitnessTracker.Models
{
    public class FitnessUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required(AllowEmptyStrings = true)]
        public string FitnessGoal { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
    }
    //public class Workout
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public DateTime Date { get; set; }
    //    public int Duration { get; set; }
    //    public double? AverageHeartRate { get; set; } // Optional vital health data
    //    //public virtual FitnessUser User { get; set; }
    //    //public virtual ICollection<Exercise> Exercises { get; set; }
    //}

    //public class Exercise
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public int Sets { get; set; }
    //    public int Reps { get; set; }
    //    public double Weight { get; set; }
    //    public virtual Workout Workout { get; set; }
    //}

    public class VitalMetrics
    {
        public int Id { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double? BodyFatPercentage { get; set; } // Optional vital health data
        public virtual FitnessUser User { get; set; }
    }

}

