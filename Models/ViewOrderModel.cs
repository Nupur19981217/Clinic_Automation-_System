using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class ViewOrderModel
    {
        public int DrugId { get; set; }
        public String DrugName { get; set; }
        public int OrderID { get; set; }
        public int PatientID { get; set; }
        public String PatientName { get; set; }
        public int? OrderNo { get; set; }
        public int? OrderQtty { get; set; }

        public DateTime? OrderDate { get; set; }
        public string OrderStatus { get; set; }

        public int? SupplierId { get; set; }
        public String SuppName { get; set; }


        public List<ViewOrderModel> ListOrder { get; set; }

        public List<SelectListItem> ListSupp { get; set; }
    }
}