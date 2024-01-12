using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Webapplication.Models
{
    public class Ratings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int No{get; set;}
        public string EmployeeId{get; set;}
        public string Rating{get; set;}
        public string Feedback{get; set;}
    }

}


