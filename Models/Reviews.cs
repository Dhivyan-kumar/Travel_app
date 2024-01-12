using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webapplication.Models
{
    public class Reviews
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int No{get; set;}
        public string EmployeeId{get; set;}
        [Required]
        public string ratings{get; set;}
        [Required]
        public string feedback{get; set;}
    }
}