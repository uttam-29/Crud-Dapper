using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Web;

namespace Crud_dapper.Models
{
    public partial class Skills
    {
        public int ID { get; set; }
        public string skill { get; set; }

        public bool IsCheked { get; set; }
    }
    public class EmployeeModel
    {
        [Required]
        public int EmployeeID { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Department { get; set; }

        public string Gender { get; set; }

        [MaxLength(100)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Check email address format!")]
        [Required]
        
        //[Index(IsUnique = true)]
        [Display(Name = "Email")]
        [EmailAddress]
       // [CustomEmailVerification] //this is a custom function
        public string Email { get; set; }

        public bool IsCSharp { get; set; }

        public bool IsVBA { get; set; }

        public bool IsXamarin { get; set; }
      
        public string Skills { get; set; }

        public string Search { get; set; }

        //public string Sort { get; set; } 
        
        public bool Sorted { get; set; }

        public string sort = "ASC";

        [Required]
        public int Status { get;  } 
        public List<string> _Skills { get; set; }    

    }

}