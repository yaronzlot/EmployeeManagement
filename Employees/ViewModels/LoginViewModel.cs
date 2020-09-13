using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//* Part 70 - step 1 - Implementing login functionality -> next in Login..cshtml (login view)

namespace Employees.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me")] //Seesion cookey and persistent cookey
        public bool RememberMe { get; set; }
    }
}
