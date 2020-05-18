using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "Enter Password")]
        [MinLength(5, ErrorMessage = "Enter atleast 5 characters")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [MinLength(5, ErrorMessage = "Enter atleast 5 characters")]
        public string NewPassword { get; set; }

        //[Compare("Password", ErrorMessage = "Password doesnot match")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Password doesnot match")]
        public string ConfirmNewPassword { get; set; }

    }
}