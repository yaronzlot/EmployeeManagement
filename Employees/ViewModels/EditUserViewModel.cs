using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//* Part 86 step 2  - Edit identity user - next EditUser.cshtml
namespace Employees.ViewModels
{
    public class EditUerViewModel
    {
        public EditUerViewModel()
        {
            Roles = new List<string>(); Claims = new List<string>();
        }

        public string Id { get; set; }

        [Required(ErrorMessage = "User name is Required")]
        public String UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string City { get; set; }

        public List<string> Roles { get; set; }

        public List<string> Claims { get; set; }
    }
}
