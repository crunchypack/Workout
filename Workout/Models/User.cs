using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Workout.Models
{
    public class User
    {
        public User() { }
        [Required(ErrorMessage ="Username is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage ="Must be between 2 and 50")]
        [Display(Name ="Username")]
        public string UserName { set; get; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
         ErrorMessage = "Password have at least 8 characters, 1 uppercase letter, 1 number and one special character")]
        public string Password { set; get; }
        [Required]
        public string Email { set; get; }
        public int UserId { set; get; }
    }
}
