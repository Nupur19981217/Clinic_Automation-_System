using ClinicalAutomationSystem_MVC_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem_MVC_.Controllers
{   [Authorize]
    public class SupplierController : Controller
    {
        // GET: Supplier
       
        public ActionResult Index()
        {
            ClASDBEntities db = new ClASDBEntities();
            int MemId = Convert.ToInt32(Session["MemberId"]);
            var checkName = db.Suppliers.Where(m => m.MemberId == MemId).FirstOrDefault();
            Session["ID"] = checkName.SupplierID;
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
        public ActionResult EditProfile(SupplierModel sm)
        {
            ClASDBEntities db = new ClASDBEntities();
            if (ModelState.IsValid)
            {
                int MemId = Convert.ToInt32(Session["MemberId"]);
                var getdata = db.Suppliers.FirstOrDefault(m => m.MemberId == MemId);
                getdata.FirstName = sm.FName;
                getdata.LastName = sm.LName;
                getdata.CompanyName = sm.CompName;
                getdata.CompanyAddress = sm.CompAdd;
                db.SaveChanges();
                ViewBag.text = "Edited Successfully";
            }
            else
            {
                ViewBag.text = "Error Occurred!!";
            }
            return View(sm);
        }

        public ActionResult ViewOrders()
        {
            var memId = Convert.ToInt32(Session["MemberId"]);

            ClASDBEntities db = new ClASDBEntities();
            //var currentDocID = db.Doctors.FirstOrDefault(m => m.MemberId == memId);
            List<ViewOrderModel> lst = new List<ViewOrderModel>();
            var id = Convert.ToInt32(Session["ID"]);
            var getdata = (from od in db.PatientOrderDetails
                           join d in db.Drugs
                           on od.DrugId equals d.DrugID
                           join dd in db.DrugDeliveries
                           on od.OrderID equals dd.OrderID
                           where dd.SupplierId==id
                           select new
                           {
                               od.OrderNumber,
                               dd.SupplierId,
                               d.DrugID,
                               od.OrderID,
                               d.DrugName,
                               od.OrderDate,
                               od.Quantity,
                               od.OrderStatus
                           });
            foreach (var item in getdata)
            {
                lst.Add(new ViewOrderModel
                {
                    OrderID = item.OrderID,
                    SupplierId = item.SupplierId,
                    OrderDate = item.OrderDate,
                    DrugId = item.DrugID,
                    DrugName = item.DrugName,
                    OrderStatus = item.OrderStatus,
                    OrderQtty = item.Quantity,
                    OrderNo = item.OrderNumber
                });
            }
            ViewOrderModel dm = new ViewOrderModel();
            dm.ListOrder = lst;


            return View(dm);


        }

        public ActionResult UpdateOrders(int id, string str)
        {
            ClASDBEntities db = new ClASDBEntities();
            //DoctorAppointment obj = new DoctorAppointment();
            var getOrd = db.PatientOrderDetails.FirstOrDefault(m => m.OrderID == id);
            var getDel = db.DrugDeliveries.FirstOrDefault(m => m.OrderID == id);
            var getDrug = db.Drugs.FirstOrDefault(m => m.DrugID == getOrd.DrugId);
            var q = getDrug.TotalQuantityAvailable;
            if(str=="Delivered")
            {
                getDel.DeliveryDate = DateTime.Now;
                getOrd.OrderStatus = str;
                getDrug.TotalQuantityAvailable = q - getOrd.Quantity;
            }
            else
            {
                getOrd.OrderStatus = str;
                db.DrugDeliveries.Remove(getDel);
            }
            db.SaveChanges();

            return RedirectToAction("ViewOrders", "Supplier");
        }
    }

}