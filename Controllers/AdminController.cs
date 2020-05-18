using ClinicalAutomationSystem_MVC_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClinicalAutomationSystem_MVC_.Controllers
{   [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ClASDBEntities db = new ClASDBEntities();
            int MemId = Convert.ToInt32(Session["MemberId"]);
            var checkName = db.Admins.Where(m => m.MemberId == MemId).FirstOrDefault();
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
        public ActionResult EditProfile(AdminModel dm)
        {
            ClASDBEntities db = new ClASDBEntities();
            var memId = Convert.ToInt32(Session["MemberId"]);
            string checkRole = Convert.ToString(Session["RoleName"]);
            if (checkRole == "Admin")
            {
                var getdata = db.Admins.FirstOrDefault(m => m.MemberId == memId);
                getdata.FirstName = dm.FName;
                getdata.LastName = dm.LName;
                getdata.Address = dm.Address;
                getdata.Gender = dm.Gender;
                db.SaveChanges();
                ViewBag.text = "Edited Successfully";
            }
            else
            {
                ViewBag.text = "Error Occurred!";
            }

            return View(dm);
        }
        [Authorize] //If Controller is Authorized, no meed for this
        public ActionResult AddDrugDetails()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddDrugDetails(AddDrugModel ad)
        {
            if (ModelState.IsValid)
            {
                ClASDBEntities db = new ClASDBEntities();
                Drug d = new Drug();
                d.DrugName = ad.DName;
                d.ManufactureDate = ad.MfgDate;
                d.ExpiryDate = ad.ExpDate;
                d.SideEffect = ad.SideEff;
                d.TotalQuantityAvailable = ad.Qtty;
                d.UsedFor = ad.Use;
                d.IsDeleted = "No";

                db.Drugs.Add(d);
                db.SaveChanges();
                ViewBag.text = "Drug Inserted Succesfully";
            }
            else
            {
                ViewBag.text = "Drug Not Inserted";
            }
            return View(ad);
        }

        public ActionResult ViewEditDrug()
        {
            ClASDBEntities db = new ClASDBEntities();
            Drug d = new Drug();
            AddDrugModel adm = new AddDrugModel();
            var getdata = db.Drugs.ToList();
            List<AddDrugModel> lst1 = new List<AddDrugModel>();
            //string value = "";
            foreach (var item in getdata)
            {
                
                lst1.Add(new AddDrugModel
                {
                    DName = item.DrugName,
                    DrugId = item.DrugID,
                    MfgDate = item.ManufactureDate,
                    ExpDate = item.ExpiryDate,
                    IsDeleted = item.IsDeleted,
                    SideEff = item.SideEffect,
                    Qtty = item.TotalQuantityAvailable,
                    Use = item.UsedFor
                    
                });
            }

            adm.ListDrug = lst1;
            return View(adm);
        }

 
        public ActionResult EditDrug(int id)
        {
            ClASDBEntities db = new ClASDBEntities();
            var getdata = db.Drugs.Where(m => m.DrugID == id).FirstOrDefault();
            AddDrugModel adm = new AddDrugModel();
            adm.DName = getdata.DrugName;
            adm.DrugId = getdata.DrugID;
            adm.ExpDate = getdata.ExpiryDate;
            adm.MfgDate = getdata.ManufactureDate;
            adm.Qtty = getdata.TotalQuantityAvailable;
            adm.SideEff = getdata.SideEffect;
            adm.Use = getdata.UsedFor;
           
            adm.IsDeleted = getdata.IsDeleted;
           
            
            return View(adm);
        }

        [HttpPost]
        public ActionResult EditDrug(AddDrugModel ad, int id)
        {   if (ModelState.IsValid)
            {
                ClASDBEntities db = new ClASDBEntities();
                var d = db.Drugs.Where(m => m.DrugID == id).FirstOrDefault();
                d.DrugName = ad.DName;
                d.ManufactureDate = ad.MfgDate;
                d.ExpiryDate = ad.ExpDate;
                d.SideEffect = ad.SideEff;
                d.TotalQuantityAvailable = ad.Qtty;
                d.IsDeleted = ad.IsDeleted;
                d.UsedFor = ad.Use;
               db.SaveChanges();
                ViewBag.text = "Edited Successfully";
            }
            else
            {
                ViewBag.text = "Error Occured";
            }
            return RedirectToAction("ViewEditDrug", "Admin");
        }

        public ActionResult DeleteDrug(int id)
        {
            ClASDBEntities db = new ClASDBEntities();
            var d = db.Drugs.Where(m => m.DrugID == id).FirstOrDefault();
            d.IsDeleted = "Yes";
            db.SaveChanges();
            ViewBag.Text = "Deleted Successfully";
            return RedirectToAction("ViewEditDrug", "Admin");
        }

        public ActionResult ViewPatientOrders()
        {
            var memId = Convert.ToInt32(Session["MemberId"]);

            ClASDBEntities db = new ClASDBEntities();
            //var currentDocID = db.Doctors.FirstOrDefault(m => m.MemberId == memId);
            List<ViewOrderModel> lst = new List<ViewOrderModel>();
            
            var getdata = (from od in db.PatientOrderDetails
                           join p in db.Patients
                           on od.PatientID equals p.PatientID
                           join d in db.Drugs
                           on od.DrugId equals d.DrugID
                           select new
                           {
                               od.OrderNumber,
                               p.FirstName,
                               p.PatientID,
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
                    PatientID = item.PatientID,
                    OrderDate = item.OrderDate,
                    DrugId = item.DrugID,
                    DrugName = item.DrugName,
                    OrderStatus = item.OrderStatus,
                    OrderQtty = item.Quantity,
                    PatientName= item.FirstName,
                    OrderNo=item.OrderNumber
                });
            }
            ViewOrderModel dm = new ViewOrderModel();
            dm.ListOrder = lst;

            Supplier s = new Supplier();


            List<SelectListItem> list2 = new List<SelectListItem>();
            var getd =db.Suppliers.Select(m=>new { m.SupplierID, m.FirstName}).ToList();
            foreach (var item in getd)
            {
                list2.Add(new SelectListItem
                {
                    Text = item.FirstName,
                    Value = Convert.ToString(item.SupplierID)
                });
            }
            dm.ListSupp = list2;
            return View(dm);


        }

        public ActionResult Assign(int sid, int ono)
        {
            ClASDBEntities db = new ClASDBEntities();
            DrugDelivery dd = new DrugDelivery();

            var getdata = db.PatientOrderDetails.Where(m => m.OrderID == ono).FirstOrDefault();
            getdata.OrderStatus = "Assigned";
            dd.OrderID = ono;
            dd.SupplierId = sid;
            db.DrugDeliveries.Add(dd);
            db.SaveChanges();
            return RedirectToAction("ViewPatientOrders", "Admin");


        }
    }
}