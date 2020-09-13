using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace Employees
{
    public class Program
    {
        //Main is the entry point of the program and it configure the ASP .net core application
        //Main() method since ASP .net core app starts as console application
        //At this point it become ASP .net core application
        public static void Main(string[] args)
        {
            //CreateHostBuilder() method - impliments the IHostBuilder interface
            //Build() method - builds the web host that hosts this application 
            //Web Hosts - (IIS (w3wp.exe) InProcess, IISEXPRESS.exe InProcess, dotnet.exe OutOfProcess)
            //inprocess = one server ( best perfomance)
            //outofprocess = two servers, internal server is Kastrel and external server is IIS as proxy server for load balancer (Penatly requests)
            //Run() method - run the application on the web host and start lisning to http requests
            CreateHostBuilder(args).Build().Run();
        }

        //CreateDefaultBuilder() - static method that create the web hosts with default configuration
        //Host.CreateDefaultBuilder - Host.cs is responsble for the order  of MyKey (goto definition to see)
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args) 
            .ConfigureLogging((hostingContext , logging) => //* Part 63 - step 3
            {
                logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
                // Enable NLog as one of the Logging Provider
                logging.AddNLog();
            })
            
            //CreateDefaultBuilder says the order for MyKey display (appsettings.json, appsettings.Development.json, OS, comandline
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //UseStartup is extention method that configuraing the Startup.cs
                    webBuilder.UseStartup<Startup>();
                });
    }
}
