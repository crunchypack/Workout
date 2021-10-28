using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class Exercise
    {
        public Exercise() { }
        public int ExerciseId { set;get;}
        [Required]
        public string Name { set; get; }
        public string Category { set; get; }
        [RegularExpression("^[0-9]*\\.[0-9]{2}$", ErrorMessage ="Numbers with 2 decimals only")]
        public decimal Weight { set; get; }
        public string Muscle { set; get; }
        public int Workout { set; get; }
        
    }
}
