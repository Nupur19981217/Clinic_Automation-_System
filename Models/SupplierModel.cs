using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class SupplierModel
    {
        [Required(ErrorMessage ="Enter First Name")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Enter Last Name")]
        public string LName { get;  set; }

        [Required(ErrorMessage = "Enter Company Name")]
        public string CompName { get; set; }

        [Required(ErrorMessage = "Enter Company Address")]
        public string CompAdd { get;  set; }
    }
}