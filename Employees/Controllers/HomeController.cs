using Employees.Models;
using Employees.Pages;
using Employees.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Controllers
{
    //* the : Controller is to have bigger API that includes json data from using Microsoft.AspNetCore.Mvc framework;
    //*Dependency injection -  the HomeController has dependency to IEmployeeRepositry service 
    //* so this service is injected into the homecontroller using the constactor

   // [Route("[controller]/[action]")] // part 33 - set home as default attribute route and add to it bellow attribute routes
    [Authorize]
    public class HomeController : Controller
    {
        //* The controller handle and process the incoming http requests from the browser
        //* the 5 lines below are constactor injection - MVC common pattern programing
        //* the below is a private field created in line 23
        private readonly IEmployeeRepositry _employeeRepositry;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger logger; //* part 64 - step 2 - create in line 34 -> next step in SQLEmployeeRepositry.cs

        //* ctor + tab twice create constractor for HomeController class
        //* injection of IEmployeeRepositry by the name employeeRepositry 
        [Obsolete]
        public HomeController (IEmployeeRepositry employeeRepositry,
                IWebHostEnvironment webHostEnvironment, ILogger<HomeController> logger) //* part 53 - step4 + part 64 step1 - inject Ilogger
        {
            // LogLevel  //*part 64 - can see in its Go To Definision the order of the logs from 0-6 (set in appsettings.json)

            //* store it in a private field
            _employeeRepositry = employeeRepositry;
            this.webHostEnvironment = webHostEnvironment;
            this.logger = logger;
        }

        /*
        public string Index()
        {
            //* "Action" method Index that get the first employye mame from MocEmployeeRepositry hard code data
            return _employeeRepositry.GetEmployee(1).Name;
        }
        */
        //* part 33 Attribute Routing with the 3 lines below will work for:
        //* https://localhost:44341 https://localhost:44341/home https://localhost:44341/home/Details

      //  [Route("~/home")]
       // [Route("~/")]
       [AllowAnonymous]
        public ViewResult List()
        {
            //* https://localhost:44341/home/list Step3 (Part 27) - View/Home/List.cshtml
            //* Create var model that get list of all employees - Step3 (Part 27) - step 4 is create view (Razor View) for List method in Views/Home
            var model = _employeeRepositry.GetAllEmployees();
            return View(model);
        }

        /*
         //* The below works - https://localhost:44341/Home/Details
         public JsonResult Details()
         {
             //* The controller create the employee model object
             Employee model = _employeeRepositry.GetEmployee(1);
             return Json(model);
         }
         */

        //* to get xml format insted of json need to use ObjectResult https://localhost:44341/home/Details1
        //* in Startup.cs need to add -services.AddMvc(option => option.EnableEndpointRouting = false).AddXmlSerializerFormatters();
        //* to test in fiddler -> Composer use Get and paste the URL https://localhost:44341/home/Details1 
        //* User-Agent: Fiddler + Accept: application/xml + Host: localhost:44341 and Excute
        //* click the respone in the right panel and row on the lest panel to see XML format (part 21 in video)

        //* https://localhost:44341/home/Details ViewModel (Part 25)
        //* https://localhost:44341/home/Details/1 part 32 step 1 - routing (using parameter id insted of 1) -> Change Startup.cs

        //* part 33 - {id?} + (int? id) + (id??1) -> means that in case the parameter is null use defalut value 1
        //[Route("{id?}")]

        [AllowAnonymous]
        public ViewResult Details (int? id)
        //* part 41 - add name parameter public string ViewResult(int? id, string name)
        {
            //* part 41 - model binding maps the incoming http requset (form data, route data, query string data) by name (id and name)
            //* https://localhost:44341/home/Details/2?name=yaron -> "/Details/2" the "id" is part of route date
            //* and "name" "?name=yaron" is part of query string parameter (https://localhost:44341/home is form values)
            //return "id = " + id.Value.ToString() + " name = " + name;  
            //* The controller create homeDetailsViewModel object from ViewModels/HomeDetailsViewModel.cs and pass data to Views/Home/Details.cshtml
            //*** with ViewModel we check error on compilation and has insitilaise after @model. in Details.cshtml

            //* Request matched multiple actions resulting in ambiguity -> due to 2 Error.cshtml it failed

            //throw new Exception("Error in Details view " + System.Diagnostics.Process.GetCurrentProcess().ProcessName); //* Pert 60 step 1 - to test global exception - check Development and Production -> next in Startup.cs   

            logger.LogTrace("Trace log");
            logger.LogDebug("Debug log");
            logger.LogInformation("Information log");
            logger.LogWarning("Warnnig log");
            logger.LogError("Error log");
            logger.LogCritical("Critical log");
            

            //* Part 57 - step 1 - Handling 404 not found 
            Employee employee = _employeeRepositry.GetEmployee(id.Value);
             if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
               // Employee = _employeeRepositry.GetEmployee(1),
                Employee = employee,
                PageTitle = "Employee Details"
            };
          
            return View(homeDetailsViewModel);
        }

        //* Part 40 - step 1- Form tag helper - create form for new employee using tag helper (next step in Create.cshtml)
        [HttpGet]
        public ViewResult Create()
        {
            return View();

        }

        //* part 55 - Step 3
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepositry.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Role = employee.Role,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);

        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model) //* part 56 - step1  - update Employee
        {
            if (ModelState.IsValid) //* Part 42 - step2 model validation - next step in Create.cshtml
            {
                Employee employee = _employeeRepositry.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Role = model.Role;
                if (model.Photo != null) //* Part 56 - check if the user select new photo
                {
                    if (model.ExistingPhotoPath != null)
                    {
                      string filePath=  Path.Combine(webHostEnvironment.WebRootPath,"images",
                          model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProccessUplodedFile(model); //* part 56 - step2 - to reduce code for upload file (Right click on the method -> Quick Actions and refactoring -> Creates "ProccessUplodedFile"
                }

                _employeeRepositry.Update(employee);
                return RedirectToAction("List"); //* Part 56 - need to comment this line
            }

            return View();
        }


        //* part 41 - step 1 - the nethod Create above only create the view for create employee when we HTTP GET request
        //* and the method "Create" below create the new employee after click the Create buttun send HTTP POST request with emplotee parameter
        //* next step inIEmployeeRepositry.cs
        //** part 41 - step 4 - use the Add method form IEmployeeRepositry that we just created
        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model) //* part 53 - step3 - change Employee to EmployeeCreateViewModel 
        {
            if (ModelState.IsValid) //* Part 42 - step2 model validation - next step in Create.cshtml
            {
                string uniqeFileName = ProccessUplodedFile(model);
                
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Role = model.Role,
                    PhotoPath = uniqeFileName
                };
                _employeeRepositry.Add(newEmployee);
                 return RedirectToAction("details", new { id = newEmployee.Id }); //* Part 44 - need to comment this line
            }

            return View();
        }


        //* part delete - step 3 - next step below with HttpPost in HomeController.cs
        public ViewResult Delete(int id)
        {
            Employee employee = _employeeRepositry.GetEmployee(id);
            EmployeeDeleteViewModel employeeDeleteViewModel = new EmployeeDeleteViewModel
            {
                Id = employee.Id,
            };
            return View(employeeDeleteViewModel);
        }

        [HttpPost]
        public IActionResult Delete(EmployeeDeleteViewModel model) //* part delete - step3  - update Employee
        {
            if (ModelState.IsValid) //* Part 42 - step2 model validation - next step in Create.cshtml
            {
                Employee employee = _employeeRepositry.GetEmployee(model.Id);
 
                _employeeRepositry.Delete(employee.Id);
                return RedirectToAction("List"); //* Part 56 - need to comment this line
            }

            return View();
        }


        private string ProccessUplodedFile(EmployeeCreateViewModel model)
        {
            string uniqeFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_ = webHostEnvironment.WebRootPath, "images");
                uniqeFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqeFileName);
                using(var fileStream = new FileStream(filePath, FileMode.Create)) //* part 56 - solve file is using by another process
                {
                    model.Photo.CopyTo(fileStream);
                }
                
            }

            return uniqeFileName;
        }

        /*
          //* Method that return json data
          public JsonResult Index()
          {
              //* we return json data from new anonymous object with two properties Id and name
              return Json(new { Id=1,Name="Yaron"});;
              //return "Hello from MVC controller";
          }
          */
    }
}
