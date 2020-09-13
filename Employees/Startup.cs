using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employees.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Employees
{
    public class Startup
    {
        //* In order to read and write  "MyKey": "MyKey value from appsettings.json" need the constractor injection below
        //* ctor tab tab- create public statup() below - this create a constrator
        //* Create constractor injection - add "IConfiguration" with paramenter "configuration"   (CTRL+. Enter after IConfiguration add "using Microsoft.Extensions.Configuration") 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //* This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //* Part 48 - Step 1 - register AppDBContext class in the dependency injection to create new instance of AppDBContext
            services.AddDbContextPool<AppDBContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("AzureDBConnection"))); //* Part 48 - step 2 - goto appsettings.json to add section for connection string


            //* Part 65 - step 3 - identity user and Part 68 - step 1 - password complexity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;  //* ask for 10 characters insted of 6 in the default
               // options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppDBContext>();


            //Add MVC service to dependency injection container (need also to add MVC middleware after UseStaticFiles)
            //EnableEndpointRouting = false should be configured 
            //* AddMvc contians AddMVCCore
            //services.AddMvc(option => option.EnableEndpointRouting = false);

            //* Part 71 - Authorization
            services.AddMvc( options =>
            {
                options.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            //* to get XML format insted of json format from method Details1 in HomeController.cs need .AddXmlSerializerFormatters()
            //services.AddMvc(option => option.EnableEndpointRouting = false);

            //Register with dependency injection container "IEmployeeRepositry" interface (service)
            //services.AddSingleton<IEmployeeRepositry, MocEmployeeRepositry>();
           
            //* Part 49 - step 4 - line below make the HomeContoroller to work with SQLEmployeeRepository insted of MockEmployeeRepository line above - next step is part 50 Migrations
            services.AddScoped<IEmployeeRepositry, SQLEmployeeRepositry>();
            
            //services.AddScoped<IEmployeeRepositry, MocEmployeeRepositry>(); //*Part 44 - AddScoped - same service instance of http requset and new service instance for other http request so the total in create employee remain 4
            // services.AddTransient<IEmployeeRepositry, MocEmployeeRepositry>(); //*Part 44 - AddTransient - new service instance for all http requests - employees remain 3 after create new employee 
            //services.AddRazorPages();
        }

        //* This method gets called by the runtime. Use this method to configure the HTTP request pipeline to the web app.
        //* Configure method contains middleware - the first middleware is "UseDeveloperExceptionPage"
        //* Middleware component get requests from the web server to the first middleware and then passes the result to the 
        //* next middleware pipeline to response - then the last middleware that made the response pass back to the previuos middleware
        //* back to the web server that pass the response to the client (each middleware hold time for the request so the web server calc the whole procee time
        //* For best memory and perfomance use only required middleware (if only image use the "app.UseStaticFiles()" )
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env , ILogger<Startup> logger)
        {
            //*Part 6 and part 7 - InProcess/OutOfProcess - refering to launchSettings.json (commandName) and Employees.csproj (ASPNetCoreHostingModel)
            //* "ASPNETCORE_ENVIRONMENT": "Development" can be "Staging" or "Production" for security and perfomance
            //* "commandName": "IISExpress" and ASPNetCoreHostingModel:InProcess - Only one web server "IISEXPRESS"
            //* "commandName": "IISExpress" and ASPNetCoreHostingModel: OutOfProcess - Internal server-"Kestrel" External Server-"IISEXPRESS"
            //* "commandName": "Project" - ignore ASPNetCoreHostingModel - Only Internal web server-"Kestrel" (dotnet)
            //* "commandName": "IIS" and ASPNetCoreHostingModel:InProcess - Only one web server "IIS"
            //* "commandName": **** "IIS" and ASPNetCoreHostingModel: OutOfProcess - Internal server-"Kestrel" External Server-"IIS" (Proxy reverse for loadbalance)
            //* Change from GUI : Right click on Employees > Properties > Debug
            //if (env.IsEnvironment("UAT))  //* this for Acceptance test env. need to update ASPNETCORE_ENVIRONMENT to UAT in launchSettings.json
            if (env.IsDevelopment())
            {
                //UseDeveloperExceptionPage must be in the beginig of the pipeline middlewares
                //below we custom the number of code line count to 10 before and after the exception in the exception page in the web brwoser
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions
                {
                    SourceCodeLineCount = 10
                };
                
                app.UseDeveloperExceptionPage(developerExceptionPageOptions);
            }
            else
            {
                /* 
                 Part 58 - step 1 - Centralised 404 error handling - next step ErrorController.cs (Add > New item > Controller Class)
                 UseStatusCodePagesWithRedirects is middleware component for redirect to URL in case of page not found 404
                 Dev Tool -> CTRL+SHIFT+I > Network > All > https://localhost:44341/foo/bar redirected to https://localhost:44341/Error/404 
                 Status 302 means that URL was changed temperory to https://localhost:44341/Error/404 (bar->302 404->200)
                 Part 59 - UseStatusCodePagesWithReExecute -> Dev Tool - https://localhost:44341/foo/bar remain https://localhost:44341/foo/bar (bar->404) (can see the issue)
                 */
                //app.UseStatusCodePages();
                app.UseExceptionHandler("/Error"); //* Part 60 step 2 (error 500) - next step ErrorController.cs
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //app.UseExceptionHandler("/Error");
                //* The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
             //   app.UseHsts();
            }
            //* below are also middlewares - in case the request from the server is not for "UseStaticFiles" (image, CSS, JS, Html)
            //* it will pass the request to the next middleware "UseRouting" - saving time...
            // app.UseHttpsRedirection();

            //* UseDefaultFiles middleware must be placed before UseStaticFiles to see Default.html page from wwwroot folder

            //* UseDefaultFiles In order to dispaly Foo.html and not default options - Index.html, Default.html, index.htm
            //* Customize UseDefaultFiles middleware with DefaultFilesOptions

            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("Foo.html");

            //* pass defaultFilesOptions with Foo.html as parameter to UseDefaultFiles
            //app.UseDefaultFiles(defaultFilesOptions);

            //* UseStaticFiles middleware used to server (by default) static files uner wwwroor folder
            //* https://localhost:44341/ and https://localhost:44341/home ->shows Hello from MVC controller
            //* https://localhost:44341/index -> shows MW3: request handled and response produced
            app.UseStaticFiles();

            //* Right click on below -> Defenition will show that MVC request go by default to Index in HomeController.cs '{controller=Home}/{action=Index}/{id?}'.

            //app.UseMvcWithDefaultRoute();
            //* Part 32 - step2 - Routing (https://localhost:44341/home/Details/1)(https://localhost:44341/{controller}/{action}/{id})
            //* {id?} for optional so https://localhost:44341/home/Details will work
            //* {controller=home}/{action=list}/{id?}" for default route https://localhost:44341

            app.UseAuthentication(); //* Part 65 - step 3 - identity user -> next step in AppDBConext 

            app.UseMvc(routes => {
                routes.MapRoute("default", "{controller=home}/{action=list}/{id?}");
            });
           


           //p.UseMvc(); // part 33 atribute routing

            //* UseFileServer (Combines UseDefaultFiles and UseStaticFiles) to dispaly Foo.html and not default options - Index.html, Default.html, index.htm

           /*
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("Foo.html");
            //* pass defaultFilesOptions with Foo.html as parameter to UseDefaultFiles
            app.UseFileServer(fileServerOptions);  //run ctrl_F5 and on chrome CTRL+R to refresh cashe
            */

                //* UseFileServer (Combines UseDefaultFiles and UseStaticFiles) and shows Default.html in wwwroot
                //* need to comment this if we want to see the message from "app.Run(async (context) =>" in this page
                // app.UseFileServer();

                // app.UseRouting();

                // app.UseAuthentication();

                // app.UseAuthorization();


                // app.UseEndpoints(endpoints =>
                // {
                //* the Run execute IApplicationBuilder app (declared in the function)
                //* the parameter "conetxt" is for RequestDelegate of Run (goto definition)
                //* "next" deligate pass after its task "Hello World one" to run the next pice of middleware "Hello World two"
                /*
                app.Use(async (context ,next) =>
                {
                    //* Write message to the Response object and that message will be dispaly in the broswer with "WriteAsync"
                    //* await is a keyword
                    
                    await context.Response
                    .WriteAsync("MW1: Hello World one!\n");
                    logger.LogInformation("MW1: incoming request");
                    await next();
                    logger.LogInformation("MW1: outgonig response");
                    //* The below display the current process - IISExpress or Employees
                    //.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);

                });
                */

                /*
                //* async means anonymous method
                app.Use(async (context ,next) =>
                {
                    //* Write message to the Response object and that message will be dispaly in the broswer with "WriteAsync"
                    await context.Response
                    .WriteAsync("MW2: Hello World two!\n");
                    logger.LogInformation("MW2: incoming request");
                    await next();
                    logger.LogInformation("MW2: outgonig response");
                    //* The below command display the current process - IISExpress or Employees
                    //.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);


                });
                */

                //* async means anonymous method
                //   app.Run(async (context) =>
                //  {
                //* https://localhost:44341/abc.html -> abc.html does not exist in wwwroot so
                //* "UseDeveloperExceptionPage" should servre the exception in the developer server page but if abc.html is missing and it detect that 
                //* there is another middleware "UseFileServer" after it, it will not do anything and will pass the request to "UseFileServer"
                //*  but we know that abc.html does not exist in wwwroot folder so  
                //* "UseFileServer" will pass the incoming request to the next middleware that throw "Some error processing the request" 
                //throw new Exception("Some error processing the request"); 

                //* "Configuration" below is from constractor injection above  - IConfiguration
                //* Dispaly MyKey from appsettings.Development.json (if not commented) or appsettings.json or from Environment Variables
                //* MyKey in OS Environment Variables wins MyKey in appsettings.Development.json that wins MyKey in appsettings.json
                //* MyKey in command line wins all - dotnet run Mykey="Mykey value from command line"
                //await context.Response.WriteAsync(Configuration["MyKey"]);
                //    await context.Response.WriteAsync("Hello World - page not found 404"); //Part 17 in video 

                //logger.LogInformation("MW3: request handled and response produced");

                //* to see the value of ASPNETCORE_ENVIRONMENT in launchSettings.json (need to comment UseServerFiles)
                //await context.Response.WriteAsync(env.EnvironmentName);
                // });


                //  endpoints.MapRazorPages();
                // });

            }
        }
}
