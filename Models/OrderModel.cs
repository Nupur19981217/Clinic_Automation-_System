using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class OrderModel
    {
        [Required(ErrorMessage ="Select Drug")]
        public int DrugId { get; set; }
        public String DrugName { get; set; }
        public int? TotalQtty { get; set; }

        [Required(ErrorMessage = "Enter Quantity")]
        public int OrderQtty { get; set; }

        public int OrderID { get; set; }
        public int PatientID { get; set; }
        public int OrderNo { get; set; }
        
        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; }


    public List<SelectListItem> ListDrug { get; set; }
    public List<OrderModel> ListOrder { get; set; }
    }
}