using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class PatientModel
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }

        public int MemberId { get; set; }
       
       


        public string DoctorName { get; set; }
        public int? DoctorId { get; set; }
        public int? AppointId { get; set; }
        public DateTime? AppointDate { get; set; }
        public string AppointSub { get; set; }
        public string AppointDesc { get; set; }
        public string AppointStatus { get; set; }
        public int? PatientId { get; set; }

        public List<SelectListItem> ListD { get; set; }
        public List<PatientModel> ListApp { get; set; }

        public string MsgSub { get; set; }
        public string MsgDetail { get; set; }
        public int? MsgId { get; set; }
        public string FrmEmail { get; set; }
        public string ToEmail { get; set; }
        public DateTime? MsgDate { get; set; }
        public int ReplyId { get; set; }
        public string IsRead { get; set; }
    }
}