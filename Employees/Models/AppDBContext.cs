using Employees.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Models
{
    //* Part 47 - Step 1 install NuGet MicrosoftEntityFrameworkCore.SqlServer -> to interact with underlying database
    //* Part 65 - step 1 - install Microsoft.AspNetCore.Identity.EntityFrameworkCore for IdentityDbContext -> Next in Stratup.cs
    public class AppDBContext : IdentityDbContext<ApplicationUser>//* Part 77 - step 4 -let AppDBContext to "know" about ApplicationUser -> next Register.cshtml
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) //* Part 47 - Step 2 create constractor (ctor) for DbContext options for CN provider
            : base(options)
        {

        }

        //* part 51 - step1 - Add OnModelCreating method below with new employees data
        //* Part 51 - step2 - Open Pakage manager Console from View-> OtherWindows
        //* Part 51 - step3 - get-help about_entityframeworkcore and get-help Add-Migration
        //* Part 51 - step4 - Add-Migration SeedEmployeesTable
        //* Part 51 - step5 - Update-Database (will create the EmployeeDB)
        //* Part 51 - step6 - View -> Sql Server Object Explorer -> EmployeeDB
        //* Part 51 - step6   Change the first employee name and add more employees to "OnModelCreating"
        //* Part 51 - step7 - Add-Migration AlterEmployeesSeedData
        //* Part 51 - step8 - Update-Database (will update the EmployeeDB with the new data)
        //* Part 51 - step9 - create ModelBuilderExtentions.cs
        //* Part 52 - step10 - move the HasData New Employee list to create ModelBuilderExtentions.cs
        //* Part 52 - step11 - call modelBuilder.Seed();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//* Part 65 - step 4 - need this line before doing "Add-Migration AddingIdentity" and "Update-Database" in Package Manager Console + set EmployeeDBConnection in Stratup.cs
            modelBuilder.Seed();
        }

        public DbSet<Employee> Employees { get; set; } //* Part 47 - Step 3 - create DBset propety (prop) for "Employee" class is our only entiry type for SQL queries
        //* next step is Part 48 - in Stratup.cs
    }
}
