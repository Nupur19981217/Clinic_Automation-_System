//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClinicalAutomationSystem_MVC_.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DrugDelivery
    {
        public int DeliveryID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> SupplierId { get; set; }
        public Nullable<System.DateTime> DeliveryDate { get; set; }
    
        public virtual PatientOrderDetail PatientOrderDetail { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}