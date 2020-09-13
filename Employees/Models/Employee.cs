using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Models
{
    public class Employee
    {
        //* prop + TAB*2 is a shortcut for new property line 
        public int Id { get; set; }
        [Required] //* Part 42 - step1 model validation - next step in HomeController
        [MaxLength(50, ErrorMessage ="Name cannot exceed 50 characters" )]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$" , ErrorMessage="Invalid Email Format")]
        [Display (Name = "Office Email")]
        public string Email { get; set; }
        [Required] //* Part 43 - Select list validation
        public Dept? Role { get; set; } // Part 40 - step 3 change to enum Dept
        //* Part 51 - "Remove-Migration" for the last update after it was updated to DB is to run "Update-Database" with the last known in table dbo.__EFMigrationsHistory and then  "Remove-Migration"
        public string PhotoPath { get; set; } //Add new column for employee pic
    }
}
