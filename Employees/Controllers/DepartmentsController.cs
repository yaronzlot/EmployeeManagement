using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Controllers
{
    public class DepartmentsController
    {
        //* part 32 - step 3 - routing for another controler - 
        //* based on the routing mapping in Startup.cs (routes.MapRoute("default", "{controller=home}/{action=list}/{id?}");)
        public string List()
        {
            //* https://localhost:44341/Departments
            return "List() of DepartmentsController";
        }
        public string Details()
        {
            //* https://localhost:44341/Departments/details
            return "Details() of DepartmentsController";
        }
    }
}
