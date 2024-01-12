using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.RegularExpressions;
namespace Webapplication.Models
{
    public class Account
    {
       [Required]
       [DisplayName("Employee ID")]
       [RegularExpression(@"^[A-Z]{3}[0-9]{5}$", ErrorMessage ="ID should contain first 3 characters as capital letters and next 5 characters should be numbers")]
       public string EmployeeId{get; set;}
       [Required]
       [DisplayName("Password")]
       [MaxLength(15)]
       [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage ="Password should contain atleast 8 characters,one capital letter, one small letter, one special character(#?!@$%^&*-),one number ")]
       public string Password{get; set;}
    }
}