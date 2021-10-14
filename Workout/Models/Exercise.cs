using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class Exercise
    {
        public Exercise() { }
        public int ExerciseId { set;get;}
        public string Name { set; get; }
        public string Category { set; get; }
        public float Weight { set; get; }
        public string Muscle { set; get; }
        public WorkoutInfo Workout { set; get; }
    }
}
