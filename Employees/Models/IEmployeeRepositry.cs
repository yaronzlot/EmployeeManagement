using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Models
{
    //* the interface is for easy code maintanace and for dependency injection for esay maintain the code
    //* IEmployeeRepositry is a service that need to impliment is Startup.cs "services.AddSingleton<IEmployeeRepositry, MocEmployeeRepositry>();"
    public interface IEmployeeRepositry
    {
        Employee GetEmployee(int id);
        //* Create new method to get list of all employees - Step1 (Part 27) - step 2 is in MocKEmployeeRepositry.cs
        IEnumerable<Employee> GetAllEmployees();
        //* part 41 - step 2 - Add method for "Add new Employee" and pass employee object -> next step MocKEmployeeRepositry.cs
        Employee Add(Employee employee);
        //* Part 49 - step 1 - add Update and Delete methods
        Employee Update(Employee emplloyeeChanges);
        Employee Delete(int id); //* part delete - step 1 - next step in EmployeeDeleteViewModel.cs
    }
}
