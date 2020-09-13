using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//* Part 76 step 1 - Custom validation attribute for domain email "nba.com - create folder Utilites + this class - next step in RegisterViewModel.cs
namespace Employees.Utilities
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowedDomain;

        public ValidEmailDomainAttribute(string allowedDomain) //* Part 76 step 2 - Custom validation attribute - create constracotr for allowedDomain that used in RegisterViewModel.cs 
        {
            this.allowedDomain = allowedDomain;
        }

        public override bool IsValid(object value) //the valid object is bound to email field in RegisterViewModel
        {
           string[] strings = value.ToString().Split('@'); //Part 76 step 4 - convert object value to string and split the string to get the domain name for compare
            return strings[1].ToUpper() == allowedDomain.ToUpper(); //if the strings are the same it return true
        }
    }
}
