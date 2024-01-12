using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace Webapplication.Models
{
    public class Employees
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ReimbursementNo{get; set;}
        [Required]
        [Display(Name ="Employee Id")]
        public string EmployeeId  {get; set;}
        [Required]
        [Display(Name ="Employee Name")]
        [RegularExpression(@"[a-zA-Z ]*$",ErrorMessage ="Employee name should not contain numbers")]
        public string EmployeeName{get; set;}
        [Display(Name ="Start Date")]
         [Required(ErrorMessage = "The start date is required")]
        public string StartDate{get; set;}
        [Display(Name ="End Date")]
        [Required(ErrorMessage = "The end date is required")]
        public string EndDate{get; set;}
        [Required]
        [Display(Name ="Project title")]
        [RegularExpression(@"[a-zA-Z ]*$",ErrorMessage ="Project Title should not contain numbers")]
        public string ProjectTitle{get; set;}
        [Required(ErrorMessage = "Trip Country/City is required")]
        [Display(Name ="Country")]
        public string TripTo{get; set;}
         [Required(ErrorMessage = "Expense-type is required")]
         [Display(Name ="Expense type")]
        public string Description1{get; set;}
         [Required(ErrorMessage = "Cost is required")]
         [Display(Name ="Cost")]
        public decimal Cost1{get; set;}
        [Required(ErrorMessage = "Expense-type is required")]
        [Display(Name ="Expense type")]
        public string Description2{get; set;}
         [Required(ErrorMessage = "Cost is required")]
         [Display(Name ="Cost")]
        public decimal? Cost2{get; set;}
         [Required(ErrorMessage = "Expense-type is required")]
         [Display(Name ="Expense type")]
        public string Description3{get; set;}
         [Required(ErrorMessage = "Cost is required")]
         [Display(Name ="Cost")]
        public decimal? Cost3{get; set;}
        [Required]
        [Display(Name ="Total Cost")]
        public decimal TotalCost{get; set;}

        [Required]
        [Column(TypeName = "varchar(155)")]
        public string Attachment{get; set;}

        [NotMapped]
        [Required(ErrorMessage = "File is required")]
        public IFormFile File{get; set;}
        public DateTime SubmittedDate{get; set;}
        [Required(ErrorMessage = "Suggesstion is required")]
        public string Suggesstion{get; set;}
        public string Status{get; set;}
    }
}