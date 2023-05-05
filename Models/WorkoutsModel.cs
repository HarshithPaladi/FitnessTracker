//using FitnessTracker.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class WorkoutsModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Workout Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [Display(Name = "Calories Burned")]
        public int CaloriesBurned { get; set; }

        // Foreign key property
        public string FitnessUserId { get; set; }

        // Navigation property
        //public FitnessUser FitnessUser { get; set; }

        // Optional vitals Navigation property
        public int? VitalsId { get; set; }
        public VitalsModel Vitals { get; set; }
    }
}