using ClinicalAutomationSystem_MVC_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem_MVC_.Controllers
{
    [Authorize]
    public class PatientController : Controller
    {
        // GET: Patient
        public ActionResult Index()
        {
            ClASDBEntities db = new ClASDBEntities();
            int MemId = Convert.ToInt32(Session["MemberId"]);
            var checkName = db.Patients.Where(m => m.MemberID == MemId).FirstOrDefault();
            Session["ID"] = checkName.PatientID;
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

            return View();

        }
        [HttpPost]
        public ActionResult EditProfile(PatientModel dm)
        {
            ClASDBEntities db = new ClASDBEntities();

            int MemId = Convert.ToInt32(Session["MemberId"]);
            var getdata = db.Patients.FirstOrDefault(m => m.MemberID == MemId);
            getdata.FirstName = dm.FName;
            getdata.LastName = dm.LName;
            getdata.Address = dm.Address;
            getdata.DOB = dm.DOB;
            getdata.Gender = dm.Gender;
            db.SaveChanges();
            ViewBag.text = "Edited Successfully";
            return View(dm);
        }
       

        public ActionResult TakeAppointment()
        {
            ClASDBEntities db = new ClASDBEntities();

            List<SelectListItem> lst1 = new List<SelectListItem>();
            var getdata = db.Doctors.Select(m => new { m.DoctorId, m.FirstName }).ToList();
            foreach (var item in getdata)
            {
                lst1.Add(new SelectListItem
                {
                    Text = item.FirstName,
                    Value = item.DoctorId.ToString()

                });
            }
            PatientModel pm = new PatientModel();
            pm.ListD = lst1;
            return View(pm);

        }

        [HttpPost]
        public ActionResult TakeAppointment(PatientModel pm)
        {
            ClASDBEntities db = new ClASDBEntities();
            List<SelectListItem> lst1 = new List<SelectListItem>();
            var getdata = db.Doctors.Select(m => new { m.DoctorId, m.FirstName }).ToList();
            foreach (var item in getdata)
            {
                lst1.Add(new SelectListItem
                {
                    Text = item.FirstName,
                    Value = item.DoctorId.ToString()

                });
            }

            pm.ListD = lst1;
            var memId = Convert.ToInt32(Session["MemberId"]);
            var pid = db.Patients.Where(m => m.MemberID == memId).FirstOrDefault();
            DoctorAppointment obj = new DoctorAppointment();
            obj.DoctorId = pm.DoctorId;
            obj.AppointmentDate = pm.AppointDate;
            obj.AppointmentStatus = "Requested";
            obj.PatientId = pid.PatientID;
            obj.Description = pm.AppointDesc;
            obj.Subject = pm.AppointSub;
            db.DoctorAppointments.Add(obj);
            db.SaveChanges();
            ViewBag.text = "Appointment Requested";
            return View(pm);

        }

        public ActionResult ViewAppointments()
        {
            var memId = Convert.ToInt32(Session["MemberId"]);
            ClASDBEntities db = new ClASDBEntities();
            var currentPat = db.Patients.FirstOrDefault(m => m.MemberID == memId);
            List<PatientModel> lst = new List<PatientModel>();
            DoctorAppointment obj = new DoctorAppointment();
            var getdata = (from m in db.DoctorAppointments
                           join t in db.Doctors
                           on m.DoctorId equals t.DoctorId
                           where (currentPat.PatientID==m.PatientId)
                           select new
                           {
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
                lst.Add(new PatientModel
                {
                    DoctorId = item.DoctorId,
                    PatientId = item.PatientId,
                    AppointDate = item.AppointmentDate,
                    AppointSub = item.Subject,
                    AppointDesc = item.Description,
                    AppointStatus = item.AppointmentStatus,
                    DoctorName = item.FirstName

                });
            }
            PatientModel pm = new PatientModel();
            pm.ListApp = lst;
            return View(pm);


        }

        
        public ActionResult SendMessageTo()
        {
            var memId = Convert.ToInt32(Session["MemberId"]);
            ClASDBEntities db = new ClASDBEntities();
            PatientModel am = new PatientModel();
            //var currentPat = db.Patients.FirstOrDefault(m => m.MemberID == memId);
            List<PatientModel> lst1 = new List<PatientModel>();
            var patId = Convert.ToInt32(Session["ID"]);
            var getdata = (from a in db.Doctors
                           join d in db.DoctorAppointments
                           on a.DoctorId equals d.DoctorId

                           where (d.AppointmentStatus == "Accepted" && d.PatientId == patId)
                           select new { a.FirstName, d.AppointmentId, d.AppointmentDate, d.AppointmentStatus }).ToList();

            foreach (var item in getdata)
            {
                lst1.Add(new PatientModel
                {
                    //DoctorId=item.DoctorId,
                    AppointId = item.AppointmentId,
                    DoctorName = item.FirstName,
                    AppointDate = item.AppointmentDate
                });
            }
            am.ListApp = lst1;
            return View(am);


        }


        public ActionResult Inbox(int id)
        {
            PatientModel pm = new PatientModel();
            pm.AppointId = id;
            return View(pm);
        }


        [HttpPost]
        public ActionResult Inbox( PatientModel pm)
        {
            ClASDBEntities db = new ClASDBEntities();
            Inbox obj = new Inbox();
            var memId = Convert.ToInt32(Session["MemberId"]);
            var sender = db.MemberLogins.Where(m => m.MemberID == memId).FirstOrDefault();
            //Id = Convert.ToInt32(Id);
            pm.FrmEmail = sender.EmailId;
            //if(ModelState.IsValid)
            //{
            //var sender = Session["EmailId"].ToString();
            //string sender = db.MemberLogins.Where(m => m.MemberID == memId).Select(a => new { a.EmailId }).ToString();
            var doc = (from da in db.DoctorAppointments
                       join d in db.Doctors
                       on da.DoctorId equals d.DoctorId
                       join m in db.MemberLogins
                       on d.MemberId equals m.MemberID
                       where da.AppointmentId == pm.AppointId 
                       select new { m.EmailId}).FirstOrDefault();
               
                obj.Subject = pm.MsgSub;
                obj.MessageDetail = pm.MsgDetail;
                obj.MessageDate = System.DateTime.Now;
                
                obj.IsRead = "0";
                obj.FromEmailId = sender.EmailId;
                obj.ToEmailId = doc.EmailId;
                
                db.Inboxes.Add(obj);
                db.SaveChanges();
                ViewBag.text = "Message Sent";
            //}
            //else
            //{
            //    ViewBag.Text = "Message not sent";
            //}
                    
           
            return View(pm);

        }

        public ActionResult ViewMessage()
        {
            var patEmailId = Convert.ToString(Session["EmailId"]);
            ClASDBEntities db = new ClASDBEntities();
            PatientModel pm = new PatientModel();
            //var currentPat = db.Patients.FirstOrDefault(m => m.MemberID == memId);
            List<PatientModel> lst1 = new List<PatientModel>();
            //var patId = Convert.ToInt32(Session["ID"]);
            var getdata = (from i in db.Inboxes
                           join m in db.MemberLogins
                           on i.ToEmailId equals m.EmailId
                           join d in db.Doctors
                           on m.MemberID equals d.MemberId
                           where (i.ReplyId==null && i.FromEmailId==patEmailId)
                           select new 
                           { 
                                d.FirstName,
                                i.MessageDate,
                                i.Subject,
                                i.MessageDetail,
                                i.MessageId
                               
                           }).ToList();

            foreach (var item in getdata)
            {
                lst1.Add(new PatientModel
                {
                    //DoctorId=item.DoctorId,
                    MsgId = item.MessageId,
                    DoctorName = item.FirstName,
                    MsgDate = item.MessageDate,
                    MsgSub=item.Subject,
                    MsgDetail=item.MessageDetail
                });
            }
            pm.ListApp = lst1;
            return View(pm);


        }

        public ActionResult ViewAllMessages(int id)
        {
            ClASDBEntities db = new ClASDBEntities();
            PatientModel pm = new PatientModel();
            List<PatientModel> lst1 = new List<PatientModel>();
            //var patId = Convert.ToInt32(Session["ID"]);
            var getdata = db.Inboxes.Where(m => m.ReplyId==id).Select(a => new { a.MessageDate, a.MessageDetail,a.Subject, a.MessageId, a.ToEmailId, a.FromEmailId }).ToList();
                          
            foreach (var item in getdata)
            {
                lst1.Add(new PatientModel
                {
                    //DoctorId=item.DoctorId,
                    MsgId = item.MessageId,
                    FrmEmail=item.FromEmailId,
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
        public ActionResult ViewAllMessages(PatientModel pm,int id)
        {
            ClASDBEntities db = new ClASDBEntities();
            Inbox obj = new Inbox();
            var sender = Convert.ToString(Session["EmailId"]);
            var memId = Convert.ToInt32(Session["MemberId"]);
            var getdata = db.Inboxes.FirstOrDefault(m => m.MessageId == id);
            obj.FromEmailId = sender;
            obj.ToEmailId = getdata.ToEmailId;
            obj.MessageDetail = pm.MsgDetail;
            obj.MessageDate = DateTime.Now;
            obj.ReplyId = id;

            List<PatientModel> lst1 = new List<PatientModel>();
            //var patId = Convert.ToInt32(Session["ID"]);
            var getd = db.Inboxes.Where(m => m.ReplyId == id).Select(a => new { a.MessageDate, a.MessageDetail, a.Subject, a.MessageId, a.ToEmailId, a.FromEmailId }).ToList();

            foreach (var item in getd)
            {
                lst1.Add(new PatientModel
                {
                    //DoctorId=item.DoctorId,
                    MsgId = item.MessageId,
                    FrmEmail = item.FromEmailId,
                    MsgDate = item.MessageDate,
                    MsgSub = item.Subject,
                    MsgDetail = item.MessageDetail
                });
            }
            pm.ListApp = lst1;

            db.Inboxes.Add(obj);
            db.SaveChanges();
            return View(pm);
        }
        public ActionResult OrderDrug()
        {
            ClASDBEntities db = new ClASDBEntities();
            List<SelectListItem> listD = new List<SelectListItem>();
            var getdata =(from d in db.Drugs where d.IsDeleted!="Yes" select new { d.DrugID, d.DrugName}).ToList();
            foreach(var item in getdata)
            {
                listD.Add(new SelectListItem
                {
                    Text= item.DrugName,
                    Value=Convert.ToString(item.DrugID)
                });
            }
            OrderModel od = new OrderModel();
            od.ListDrug = listD;
            return View(od);
        }
        public ActionResult getQuant(int id)
        {
            ClASDBEntities db = new ClASDBEntities();
            //OrderModel od = new OrderModel();
            var getd = db.Drugs.Where(m => m.DrugID == id).FirstOrDefault();
            var totalQtty = getd.TotalQuantityAvailable;

            return Json(totalQtty,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OrderDrug(OrderModel od)
        {
            ClASDBEntities db = new ClASDBEntities();
            PatientOrderDetail pod = new PatientOrderDetail();


            List<SelectListItem> listD = new List<SelectListItem>();
            var getdata = (from d in db.Drugs where d.IsDeleted =="No" select new { d.DrugID, d.DrugName }).ToList();
            foreach (var item in getdata)
            {
                listD.Add(new SelectListItem
                {
                    Text = item.DrugName,
                    Value = Convert.ToString(item.DrugID)
                });
            }
            od.ListDrug = listD;
            Random r = new Random();
                   
            var patId = Convert.ToInt32(Session["ID"]);
            var getd = db.Drugs.Where(m => m.DrugID == od.DrugId).FirstOrDefault();
            var getQtty = getd.TotalQuantityAvailable;
            //od.TotalQtty = getQtty;
            if(ModelState.IsValid)
            {
                if(od.OrderQtty==0)
                {
                    ViewBag.text = "Enter order quantity more than 0!";
                }
                else if( od.OrderQtty<=getQtty)
                {
                    //getd.TotalQuantityAvailable = getQtty - od.OrderQtty;
                    ViewBag.text = "Placing Order...";
                    pod.DrugId = od.DrugId;
                    pod.OrderStatus = "Requested";
                    pod.PatientID = patId;
                    pod.Quantity = od.OrderQtty;
                    pod.OrderNumber = r.Next(1, 1000);
                    pod.OrderDate = DateTime.Now;
                    db.PatientOrderDetails.Add(pod);
                    db.SaveChanges();
                   
                }
                else
                {
                    ViewBag.text = "Enter order quantity less than available quantity!";
                }
                ViewBag.text2 = "Order Placed!";
            }
            else
            {
                ViewBag.text2 = "Error Occurred!";
            }
            return View(od);

        }

        public ActionResult ViewOrders()
        {
            var patEmailId = Convert.ToString(Session["EmailId"]);
            ClASDBEntities db = new ClASDBEntities();
            ViewOrderModel pm = new ViewOrderModel();
            //var currentPat = db.Patients.FirstOrDefault(m => m.MemberID == memId);
            List<ViewOrderModel> lst1 = new List<ViewOrderModel>();
            var patId = Convert.ToInt32(Session["ID"]);
            var getdata = (from d in db.Drugs
                           join pod in db.PatientOrderDetails
                           on d.DrugID equals pod.DrugId
                           where (pod.PatientID == patId)
                           select new
                           {
                               d.DrugName,
                               pod.OrderDate,
                               pod.OrderStatus,
                               pod.Quantity,
                               pod.OrderID

                           }).ToList();

            foreach (var item in getdata)
            {
                lst1.Add(new ViewOrderModel
                {
                    //DoctorId=item.DoctorId,
                    OrderID = item.OrderID,
                    DrugName = item.DrugName,
                    OrderDate = item.OrderDate,
                    OrderStatus = item.OrderStatus,
                     OrderQtty= item.Quantity
                });
            }
            pm.ListOrder = lst1;
            return View(pm);


        }


    }
}