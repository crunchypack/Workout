using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class ViewModels
    {
        public IEnumerable<User> users { set; get; }
        public IEnumerable<WorkoutInfo> workouts { set; get; }
    }
}
