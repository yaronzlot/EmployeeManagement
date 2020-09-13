using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//* Part 77 - step 1 - Extend IdentityUser
namespace Employees.Models
{
    public class ApplicationUser : IdentityUser //* Part 77 - step 2 - right click on "IdentityUser">Find All References and repleace with "ApplicationUser"
    {
        public string City { get; set; } //* Part 77 - step 3 - Add new property that does not exist in IdentityUser - next step in AppDBContext.cs (AspNetUsers)
    }
}
