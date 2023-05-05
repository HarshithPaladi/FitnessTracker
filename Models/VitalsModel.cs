using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class VitalsModel
    {
        [Key]
        public int VitalsId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public int? HeartRate { get; set; }

        public int? SystolicBP { get; set; }

        public int? DiastolicBP { get; set; }

        public int? OxygenSaturation { get; set; }

        // Reference to FitnessUser
        public string FitnessUserId { get; set; }

        public int? WorkoutsId { get; set; }
    }
}