using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class DataModel
    {
        [Required(ErrorMessage = "Enter Role")]
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Enter email")]
        [EmailAddress(ErrorMessage = "incorrect")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [MinLength(5,ErrorMessage ="Enter atleast 5 characters")]
        public string Password { get; set; }

        //[Compare("Password", ErrorMessage = "Password doesnot match")]
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage= "Password doesnot match")]
        public string ConfirmPassword { get; set; }

        /////////////////////////////////////////////////////////////////////////////////////////////////////


          
        public List<SelectListItem> ListR { get; set; }
    }
}