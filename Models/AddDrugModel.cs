using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class AddDrugModel
    {
        public int DrugId { get; set; }
        public string DName { get; set; }
        public string Use { get; set; }
        public DateTime? MfgDate { get; set; }
        public DateTime? ExpDate { get; set; }
        public string SideEff { get; set; }
        public int? Qtty { get; set; }
        public string IsDeleted { get; set; }
        public List<AddDrugModel> ListDrug{get; set;} 
    }
}