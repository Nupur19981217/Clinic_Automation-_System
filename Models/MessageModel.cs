using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicalAutomationSystem_MVC_.Models
{
    public class MessageModel
    {
        public List<MessageModel> ListApp { get; set; }

        public string MsgSub { get; set; }
        public string MsgDetail { get; set; }
        public int? MsgId { get; set; }
        public string FrmEmail { get; set; }
        public string ToEmail { get; set; }
        public DateTime? MsgDate { get; set; }
        public int ReplyId { get; set; }
        public string IsRead { get; set; }
        public string PatientName { get; set; }
    }
}