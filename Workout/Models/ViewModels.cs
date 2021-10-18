using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class ViewModels
    {
        public IEnumerable<User> Users { set; get; }
        public IEnumerable<WorkoutInfo> Workouts { set; get; }
        public IEnumerable<Exercise> Exercises{ set; get; }
    }
}
