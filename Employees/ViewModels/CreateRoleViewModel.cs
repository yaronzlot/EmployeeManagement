using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.ViewModels
{
    //* Part 78 step 2 - Creating Roles - next back to AdministrationController.cs
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
