using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Employees.Models
{
    //* Implemnation of IEmployeeRepositry (Provide interface to IEmployeeRepositry) for more flexiable and easy for unit test
    public class MocEmployeeRepositry : IEmployeeRepositry
    {
        //* _employeelist is a privte field and its type is List of Employee class in Employee.cs
        private List<Employee> _employeelist;

        //* Constractor for class MocEmployeeRepositry
        public MocEmployeeRepositry()
        {
            //* create new list "_employeelist" and add data into it
            _employeelist = new List<Employee>()
            {
                //* for implemenation of IEmployeeRepositry we addedsome hard coded data (next will be from SQL)
                new Employee() { Id = 1, Name = "michael", Role = Dept.PointGuard, Email = "michael@nba.com"},
                new Employee() { Id = 2, Name = "denis", Role = Dept.PowerForward, Email = "denis@nba.com" },
                new Employee() { Id = 3, Name = "scottie", Role = Dept.SmallForward, Email = "scottie@nba.com" }
            };
        }

        //* part 41 - step 3 - Add implemnation for Create new Employee
        public Employee Add(Employee employee)
        {
            employee.Id = _employeelist.Max(e => e.Id) + 1;
            _employeelist.Add(employee);
            return (employee);
        }

        //* Part 49 - step 2 - add Delete and Update (below) methods to in memory repository in this mock
        public Employee Delete(int id)
        {
           Employee employee = _employeelist.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeelist.Remove(employee);
            }
            return employee;
        }

        //* Create new method to get list of all employees - Step2 (Part 27) - step 3 is in HomeController.cs List method
        //* The IEmployeeRepositry above was in red and click on it + CTRL+. implemnt the interface below 
        //* the private field _employeelist was create auto with the previous action
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeelist;
        }

        public Employee GetEmployee(int id)
        {
            //* return the first employee element
           return _employeelist.FirstOrDefault(e => e.Id == id);
            // throw new NotImplementedException();
        }

        //* Part 49 - step 2 - add Update and Delete methods to in memory repository in this mock -> next step add new class in Models "SQLEmployeeRepositry"
        public Employee Update(Employee emplloyeeChanges)
        {
            Employee employee = _employeelist.FirstOrDefault(e => e.Id == emplloyeeChanges.Id);
            if (employee != null)
            {
                employee.Name = emplloyeeChanges.Name;
                employee.Email = emplloyeeChanges.Email;
                employee.Role = emplloyeeChanges.Role;
            }
            return employee;
        }
    }
}
