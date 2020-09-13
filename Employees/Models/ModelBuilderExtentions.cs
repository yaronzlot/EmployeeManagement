using Employees.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Models
{
    public static class ModelBuilderExtentions
    {
        //* part 51 step 9-10 for extention method (ModelBuilder) the class should be static
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   Id = 1,
                   Name = "Michael Jordan",
                   Role = Dept.PointGuard,
                   Email = "michael@nba.com"
               },
               new Employee
               {
                   Id = 2,
                   Name = "Dennis Rodman",
                   Role = Dept.SmallForward,
                   Email = "dennis@nba.com"
               },
               new Employee
               {
                   Id = 3,
                   Name = "Scottie Pippen",
                   Role = Dept.PowerForward,
                   Email = "scottie@nba.com"
               }
            );
        }
    }
}
