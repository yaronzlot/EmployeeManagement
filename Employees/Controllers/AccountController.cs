using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employees.Models;
using Employees.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//* Part 66 - step 1 - Register new user with asp net core identity -> next step Create Accout folder under Views and create Register.cshtml
namespace Employees.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        //* Part 66 - step 5 - ctor + tab*2
        public AccountController(UserManager<Models.ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //* Part 69 step 4 - Show or hide login and logout links based on login status

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("list", "home");
        }

        //* Part 66 - step 4
        [HttpGet]
        [AllowAnonymous] // part 71 - Authorization 
        public IActionResult Register()
        {
            return View();
        }

        //* Part 75 - step 1 - remote validation - check if the email is in use -> next step in RegisterViewModel.cs
        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if(user == null) 
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        [HttpPost]
        [AllowAnonymous] // part 71 - Authorization 
        //* Part 67 - step 1 - UseManager and SigninManager + async and await (so the program will not hang)
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                  UserName = model.Email,
                  Email = model.Email,
                  City = model.City //* Part 77 - step 7 - add City field
                };
                var result = await userManager.CreateAsync(user, model.Password); 

                if (result.Succeeded)
                {
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin")) //* Part 84 step 3 - list  all users
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("list", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //* Part 70 - step 3 - Implementing login functionality
        [HttpGet]
        [AllowAnonymous] // part 71 - Authorization 
        public IActionResult Login()
        {
            return View(); //goto/show Login.cshtml (login view/action)
        }

        [HttpPost]
        [AllowAnonymous] // part 71 - Authorization 
        //Part 74 - to check that the input validation in Login is on the client side set break point in line 81
        public async Task<IActionResult> Login(LoginViewModel model , string returnURL) //to see returnURL when try to Create https://meyerweb.com/eric/tools/dencoder/
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password,
                    model.RememberMe,false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnURL) && Url.IsLocalUrl(returnURL))//Part 72 - Redirect user to original url after login (!string = if string is not)
                    {
                        return Redirect(returnURL); //Part 73 - using "Redirect" is security hole!!! "open redirect vulnerability" - the return URL can be set to the attacker website - solved by check "Url.IsLocalUrl(returnURL)"
                    }
                    else
                    {
                        return RedirectToAction("list", "home");
                    }
                }
                ModelState.AddModelError("", "Invalid Login Attampt");
            }
            return View(model);
        }
        //* Part 83 - Show or hide navigation menu based on user role
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
