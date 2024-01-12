using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Webapplication.Models;

namespace Webapplication.Models
{
    public class ForgotPassword
    {
        [Required]
        [RegularExpression(@"^[A-Z]{3}[0-9]{5}$",ErrorMessage ="first 3 characters should be capital alphabets next 5 characters should be numbers ")]
        public string EmployeeID{get; set;}
        [Required]
        [RegularExpression(@"^[a-z0-9]+@[a-z0-9.]+$",ErrorMessage ="Emailid should contain letters, numbers before @ and after @ contains domain name")]
        public string Emailaddress{get; set;}
        [Required]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage ="Password should contain atleast 8 characters,one capital letter, one small letter, one special character(#?!@$%^&*-),one number ")]
        public string Password{get; set;}
        [Required]
        [Compare("Password",ErrorMessage ="Password doesnot match")]
        public string ConfirmPassword{get; set;}
    }   
}