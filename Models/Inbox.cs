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
    
    public partial class Inbox
    {
        public int MessageId { get; set; }
        public string FromEmailId { get; set; }
        public string ToEmailId { get; set; }
        public string Subject { get; set; }
        public string MessageDetail { get; set; }
        public Nullable<System.DateTime> MessageDate { get; set; }
        public Nullable<int> ReplyId { get; set; }
        public string IsRead { get; set; }
    }
}
