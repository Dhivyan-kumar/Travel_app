using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
namespace Webapplication.Models
{
    public class SignUpAccount
    {
        [Required]
        [RegularExpression(@"^[A-Z]{3}[0-9]{5}$", ErrorMessage ="ID should contain first 3 characters as capital letters and next 5 characters should be numbers")]
        public string EmployeeID{get; set;}
        [Required]
        [RegularExpression(@"^[a-zA-Z ]*$",ErrorMessage ="Name should not contain any numbers")]
        public string EmployeeName{get; set;}
        [Required]
        public string Emailaddress{get; set;}
            [Required]
            [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage ="Password should contain atleast 8 characters,one capital letter, one small letter, one special character(#?!@$%^&*-),one number ")]
            public string Password{get; set;}
        [Required]
        [Compare("Password",ErrorMessage ="Password doesnot match")]
        public string ConfirmPassword{get; set;}
        [Required]
        public string Role{get; set;}

    }
}