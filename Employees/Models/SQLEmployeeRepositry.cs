using Employees.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Models
{
    //* Part 49 - step 3 - add all methods to SQL repository by creating this casll and impement IEmployeeRepositry
    //* Next step is in Startup.cs to make the HomeContoroller to work with SQLEmployeeRepository insted of MockEmployeeRepository

    public class SQLEmployeeRepositry : IEmployeeRepositry
    {
        private readonly AppDBContext context;
        private readonly ILogger<SQLEmployeeRepositry> logger;

        //Part 64 - step 3 -> Inject Ilogger
        public SQLEmployeeRepositry(AppDBContext context, ILogger<SQLEmployeeRepositry> logger) //* Part 49 - step 4 - create constractor (ctor)AppDBContext for and click CTRL+. after context to crate property
        {
            this.context = context;
            this.logger = logger;
        }
        public Employee Add(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return (employee);
        }

        public Employee Delete(int id)
        {
            Employee employee=  context.Employees.Find(id);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            //* Part 64 - step 4
            logger.LogTrace("Trace log");
            logger.LogDebug("Debug log");
            logger.LogInformation("Information log");
            logger.LogWarning("Warnnig log");
            logger.LogError("Error log");
            logger.LogCritical("Critical log");

            return context.Employees.Find(id);
        }

        public Employee Update(Employee emplloyeeChanges)
        {
            var employee = context.Employees.Attach(emplloyeeChanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return emplloyeeChanges;
        }
    }
}
