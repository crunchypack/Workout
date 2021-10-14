using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class WorkoutInfo
    {
        public WorkoutInfo() { }
        public int WorkoutId { get; set; }
        public DateTime Date { get; set; }
        public string TypeOfWorkout { get; set; }
        public User User { get; set; }
    }
}
