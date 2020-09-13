using Employees.Migrations;
using Employees.Models;
using Employees.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Controllers
{
    [Authorize(Roles = "Admin")] //Part 82 step 1

    // [Authorize(Roles = "Admin,User")] pert 82 - option for users in Admin and Users in User

    /*Part 82 option that the user must be in both Admin and User roles
    [Authorize(Roles = "Admin")]
    [Authorize(Roles = "User")]
    */
    //* Part 78 step 1 - Creating Roles - next CreateRoleViewModel
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager) //for part 80 to get users we ApplicationUser that extened IdentiryUser 
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        //* Part 84 step 1 - list  all users -> next ListUsers.cshtml
        [HttpGet]
        public IActionResult ListUsers()
        {
            var user = userManager.Users;
            return View(user);
        }

        //* Part 86 step 1  - Edit identity user - next EditUserViewModel.cs
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id); // set user variable with user properties stored in DB

            if (user == null)
            {
                ViewBag.ErrorMassage = $"User with id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUerViewModel  //pass the user properties from the DB that are in user variable now and pass them to EditUerViewModel which bind to EditUser view
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                City = user.City,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles.ToList()
            };

            return View(model); //show EditUser with EditUerViewModel user properties sotred now in user varaible
        }


        //* Part 86 step 3 - Edit Idenity user
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUerViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id); //find the user id in the DB - the comes from EditUerViewModel which binded to view EditUser

            if (user == null)
            {
                ViewBag.ErrorMassage = $"User with id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.City = model.City;
                var result = await userManager.UpdateAsync(user); //update the DB with the local variable user properties that comes from EditUser view via EditUerViewModel 

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        //* Part 78 step 3 - Creating Roles - next CreateRole.cshtml
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        //* Part 78 step 5 - Creating Roles - next
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        //* Part 79 step 1 - Get list of roles -> next ListRoles.cshtml
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        //* Part 80 step 2 - Edit role -> next EditRole.cshtml
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMassage = $"Role with id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        //* Part 80 step 4- Edit Role
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMassage = $"Role with id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }


        //* Part 81 step 3 - Add or remove users from role - next in EdirUsersInRole.cshtml
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleid)
        {
            ViewBag.roleId = roleid; //Send roleid to AdministrationController

            var role = await roleManager.FindByIdAsync(roleid);

            if (role == null)
            {
                ViewBag.ErrorMassage = $"Role with id = {roleid} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        //* Part 81 step 5 - Add or remove users from role - next in 
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleid)
        {

            var role = await roleManager.FindByIdAsync(roleid);
            {
                if (role == null)
                {
                    ViewBag.ErrorMessage = $"Role with id = {roleid} cannot be fonund";
                    return View("NotFound");
                }

                for (int i = 0; i < model.Count; i++)
                {
                    var user = await userManager.FindByIdAsync(model[i].UserId);

                    IdentityResult result = null;

                    // chack if the user was check and it is not lready included in the role
                    if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await userManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!(model[i].IsSelected) && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        result = await userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue; //in case the user was not selected and it is not in the role continue to the next user
                    }

                    if (result.Succeeded)
                    {
                        if (i < (model.Count - 1)) // if i < model.count there are more users to loop on
                            continue;
                        else
                            return RedirectToAction("EditRole", new { Id = role.Id });
                    }

                }

                return RedirectToAction("EditRole", new { Id = role.Id });
            }
        }
    }
}
