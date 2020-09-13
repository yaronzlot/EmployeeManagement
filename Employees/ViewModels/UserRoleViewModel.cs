using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//* Part 81 step 2 - Add or remove users from role - next in AdministrationController.cs
namespace Employees.ViewModels
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public bool IsSelected { get; set; }
    }
}
