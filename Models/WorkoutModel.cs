using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class WorkoutModel
    {
        [Key]
        public int WorkoutId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int Duration { get; set; }

        public int CaloriesBurned { get; set; }

        public int? AverageHeartRate { get; set; }

        public int? BloodPressureSystolic { get; set; }

        public int? BloodPressureDiastolic { get; set; }

        [Required]
        [ForeignKey("FitnessUser")]
        public string UserId { get; set; }
        public FitnessUser FitnessUser { get; set; }

    }
}
