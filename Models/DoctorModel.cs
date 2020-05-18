using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class DoctorModel
    {
        public string FName { get; set; }

        public string LName { get; set; }

        public int SpecializeId { get; set; }

         public int Experience { get; set; }

        public string Gender { get; set; }

        public List<SelectListItem> ListS { get; set; }



        public string PatientName { get; set; }
        public string PGender { get; set; }
        public int? PatientId { get; set; }
        public int? AppointId { get; set; }
        public DateTime? AppointDate { get; set; }
        public string AppointSub { get; set; }
        public string AppointDesc { get; set; }
        public string AppointStatus { get; set; }
        public int? DoctorId { get; set; }

        public List<SelectListItem> ListD { get; set; }
        public List<DoctorModel> ListApp { get; set; }
    }
}