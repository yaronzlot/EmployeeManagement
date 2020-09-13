using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//* Part 80 step 1 - Edit Role -> Next in AdministrationController.cs
namespace Employees.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string  Id { get; set; }
        
        [Required (ErrorMessage = "Role name is Required")]
        public String RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
