using ClinicalAutomationSystem_MVC_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ClinicalAutomationSystem_MVC_.Controllers
{
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult Index()
        {
            ClASDBEntities db = new ClASDBEntities();
            int MemId = Convert.ToInt32(Session["MemberId"]);
            var checkName = db.Doctors.Where(m => m.MemberId == MemId).FirstOrDefault();
            if (checkName.FirstName != null)
            {
                Session["name"] = checkName.FirstName;
            }
            else
            {
                Session["name"] = null;
            }
            return View();
        }

        public ActionResult EditProfile()
        {
            ClASDBEntities db = new ClASDBEntities();

            List<SelectListItem> lst1 = new List<SelectListItem>();
            var getdata = db.SpecializedDatas.ToList();
            foreach (var item in getdata)
            {
                lst1.Add(new SelectListItem
                {
                    Text = item.SpecializedName,
                    Value = item.SpecializedId.ToString()

                });
            }
            DoctorModel dt = new DoctorModel();
            dt.ListS = lst1;
            return View(dt);

        }
        [HttpPost]
        public ActionResult EditProfile(DoctorModel dm)
        {
            ClASDBEntities db = new ClASDBEntities();
            List<SelectListItem> lst1 = new List<SelectListItem>();
            var getd = db.SpecializedDatas.ToList();
            foreach (var item in getd)
            {
                lst1.Add(new SelectListItem
                {
                    Text = item.SpecializedName,
                    Value = item.SpecializedId.ToString()

                });
            }
            int MemId = Convert.ToInt32(Session["MemberId"]);
            var getdata = db.Doctors.FirstOrDefault(m => m.MemberId == MemId);
            getdata.FirstName = dm.FName;
            getdata.LastName = dm.LName;
            getdata.SpecislizeId = dm.SpecializeId;
            getdata.TotalExperience = dm.Experience;
            getdata.Gender = dm.Gender;



            DoctorModel dt = new DoctorModel();
            dt.ListS = lst1;
            db.SaveChanges();
            ViewBag.text = "Edited Successfully";
            return View(dt);
        }
        //[HttpPost]
        public ActionResult ViewAppointments()
        { var memId = Convert.ToInt32(Session["MemberId"]);
            
            ClASDBEntities db = new ClASDBEntities();
            var currentDocID = db.Doctors.FirstOrDefault(m => m.MemberId == memId);
            List<DoctorModel> lst = new List<DoctorModel>();
            //DoctorAppointment obj = new DoctorAppointment();
            var getdata = (from m in db.DoctorAppointments
                           join t in db.Patients
                           on m.PatientId equals t.PatientID
                           where(m.DoctorId==currentDocID.DoctorId)
                           select new
                           {    m.AppointmentId,
                               m.AppointmentDate,
                               m.DoctorId,
                               m.Subject,
                               m.Description,
                               m.AppointmentStatus,
                               m.PatientId,
                               t.FirstName
                           });
            foreach (var item in getdata)
            {
                lst.Add(new DoctorModel
                {
                    DoctorId = item.DoctorId,
                    PatientId = item.PatientId,
                    AppointDate = item.AppointmentDate,
                    AppointSub = item.Subject,
                    AppointDesc = item.Description,
                    AppointStatus = item.AppointmentStatus,
                    PatientName = item.FirstName,
                    AppointId=item.AppointmentId
                });
            }
            DoctorModel dm = new DoctorModel();
            dm.ListApp = lst;
            return View(dm);


        }

        public ActionResult UpdateAppointments(int id, string str)
        {
            ClASDBEntities db = new ClASDBEntities();
            //DoctorAppointment obj = new DoctorAppointment();
            var getApp = db.DoctorAppointments.FirstOrDefault(m => m.AppointmentId == id);
        
            getApp.AppointmentStatus = str;
            db.SaveChanges();

            return RedirectToAction("ViewAppointments", "Doctor");
        }

        public ActionResult ViewMessage()
        {
            var docEmailId = Convert.ToString(Session["EmailId"]);
            ClASDBEntities db = new ClASDBEntities();
            MessageModel pm = new MessageModel();
            //var currentPat = db.Patients.FirstOrDefault(m => m.MemberID == memId);
            List<MessageModel> lst1 = new List<MessageModel>();
            //var patId = Convert.ToInt32(Session["ID"]);
            var getdata = (from i in db.Inboxes
                           join m in db.MemberLogins
                           on i.FromEmailId equals m.EmailId
                           join p in db.Patients
                           on m.MemberID equals p.MemberID
                           where (i.ReplyId == null && i.ToEmailId == docEmailId)
                           select new
                           {
                               p.FirstName,
                               i.MessageDate,
                               i.Subject,
                               i.MessageDetail,
                               i.MessageId

                           }).ToList();

            foreach (var item in getdata)
            {
                lst1.Add(new MessageModel
                {
                    //DoctorId=item.DoctorId,
                    MsgId = item.MessageId,
                    PatientName = item.FirstName,
                    MsgDate = item.MessageDate,
                    MsgSub = item.Subject,
                    MsgDetail = item.MessageDetail
                });
            }
            pm.ListApp = lst1;
            return View(pm);


        }

        public ActionResult ViewAllMessages(int id)
        {
            ClASDBEntities db = new ClASDBEntities();
            MessageModel pm = new MessageModel();
            List<MessageModel> lst1 = new List<MessageModel>();
            //var patId = Convert.ToInt32(Session["ID"]);
            var getdata = db.Inboxes.Where(m => m.ReplyId == id).Select(a => new { a.MessageDate, a.MessageDetail, a.Subject, a.MessageId, a.ToEmailId, a.FromEmailId }).ToList();

            foreach (var item in getdata)
            {
                lst1.Add(new MessageModel
                {
                    //DoctorId=item.DoctorId,
                    MsgId = item.MessageId,
                    FrmEmail = item.FromEmailId,
                    ToEmail=item.ToEmailId,
                    MsgDate = item.MessageDate,
                    MsgSub = item.Subject,
                    MsgDetail = item.MessageDetail
                });
            }
            pm.ListApp = lst1;
            return View(pm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ViewAllMessages(MessageModel pm, int id)
        {
            ClASDBEntities db = new ClASDBEntities();
            Inbox obj = new Inbox();
          
            List<MessageModel> lst1 = new List<MessageModel>();
            //var patId = Convert.ToInt32(Session["ID"]);
            var getdata = db.Inboxes.Where(m => m.ReplyId == id).Select(a => new { a.MessageDate, a.MessageDetail, a.Subject, a.MessageId, a.ToEmailId, a.FromEmailId }).ToList();

            foreach (var item in getdata)
            {
                lst1.Add(new MessageModel
                {
                    //DoctorId=item.DoctorId,
                    MsgId = item.MessageId,
                    FrmEmail = item.FromEmailId,
                    ToEmail = item.ToEmailId,
                    MsgDate = item.MessageDate,
                    MsgSub = item.Subject,
                    MsgDetail = item.MessageDetail
                });
            }
            pm.ListApp = lst1;
            var sender = Convert.ToString(Session["EmailId"]);
            var memId = Convert.ToInt32(Session["MemberId"]);
            var getd = db.Inboxes.FirstOrDefault(m => m.MessageId == id);
            obj.FromEmailId = sender;
            obj.ToEmailId = getd.FromEmailId;
            obj.MessageDetail = pm.MsgDetail;
            obj.MessageDate = DateTime.Now;
            obj.ReplyId = id;

            db.Inboxes.Add(obj);
            db.SaveChanges();
            ViewBag.text = "Message Sent";
            return View(pm);
        }

    }
}
   