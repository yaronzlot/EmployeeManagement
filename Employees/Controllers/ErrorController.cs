using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
//Part 58 + 59 - https://localhost:44341/foo/bar?abc=123   (/foo/bar is the URL or Route and?abc=123 is the query string
// the above were added to NotFound.cshtml (/foo/bar and foo/bar?abc=123 get error 404)

namespace Employees.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        //* part 61-step1 ctor + TAB*2 for creating the constructor below + CTRL + . after "logger" for the field above
        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        // GET: /<controller>/
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested cannot be found";
                    //* part 61-step3 - use logger -> next step delete detials in NotFound.cshtml +next in appsettings.json
                    logger.LogWarning($"404 error occured. Path = {statusCodeResult.OriginalPath} " +
                        $"and QueryString {statusCodeResult.OriginalQueryString}");
                    
                    break;
                                    
            }

            return View("NotFound"); //* next step create the view "NotFound.cshtml" in /Views/Shared
        }

        //* Pert 60 step 3 - Global exception handling (500)
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //* part 61-step2 - log eceptions -> next step in Error.cshtml to remove details
            logger.LogError($"The path {exceptionHandlerPathFeature.Path} threw an ecxeption" +
                $" {exceptionHandlerPathFeature.Error.Message}");

            return View("Error"); //* return the results to Error.cshtml
        }
    }
}
