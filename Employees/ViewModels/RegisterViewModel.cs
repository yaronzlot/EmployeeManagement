using Employees.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


//* Part 66 - step 1 - Register new user with asp net core identity -> next step create AccountController

namespace Employees.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse" ,controller: "account")]  //* Part 75 - step 2 - remote validation - call IsEmailInUse from the controller
        [ValidEmailDomain(allowedDomain:"nba.com" ,ErrorMessage = "Email domain must be nba.com")] ////* Part 76 step 2 - Custom validation attribute 
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", 
            ErrorMessage = "Password and Confirmation Password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string City { get; set; } //* Part 77 - step 5 - add City 
    }
}
