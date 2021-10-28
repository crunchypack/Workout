using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class WorkoutInfo
    {
        public WorkoutInfo() { }
        public int WorkoutId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name = "Workout name")]
        public string TypeOfWorkout { get; set; }
        public User User { get; set; }
    }
}
