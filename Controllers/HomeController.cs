
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ClinicalAutomationSystem_MVC_.Models;

namespace ClinicalAutomationSystem_MVC_.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            return View();
        }

        // GET: Login

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(DataModel dt)
        {
            ClASDBEntities db = new ClASDBEntities();
            var getdata = db.MemberLogins.FirstOrDefault
                (m => m.EmailId == dt.Email && m.Password == dt.Password);


            if (getdata != null)
            {
                var getRole = db.RoleDetails.Where(m => m.RoleId == getdata.RoleId).Select(m => m.RoleName).FirstOrDefault();
                string Rolename = getRole.ToString();
                FormsAuthentication.SetAuthCookie(dt.Email, false);
                var authTicket = new FormsAuthenticationTicket(
                    1,
                    getdata.EmailId,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(20),
                    false, Rolename);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                Session["MemberId"] = getdata.MemberID;
                Session["EmailId"] = dt.Email;
                Session["RoleName"] = Rolename;
                switch (getdata.RoleId)
                {
                    case (1):
                        return RedirectToAction("Index", "Admin");
                    case (2):
                        return RedirectToAction("Index", "Doctor");
                    case (3):
                        return RedirectToAction("Index", "Patient");
                    case (4):
                        return RedirectToAction("Index", "Supplier");

                }

            }
            else
            {
                ViewBag.text = "Invalid email or password";

            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Register()
        {
            ClASDBEntities db = new ClASDBEntities();
            MemberLogin obj = new MemberLogin();
            List<SelectListItem> l = new List<SelectListItem>();
            var getdata = db.RoleDetails.ToList();
            foreach (var item in getdata)
            {
                if (item.RoleId == 1)
                {
                    continue;
                }
                else
                {
                    l.Add(new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.RoleId.ToString()

                    });
                }
            }
            DataModel dt = new DataModel();
            dt.ListR = l;
            return View(dt);
        }

        [HttpPost]
        public ActionResult Register(DataModel dt)
        {
            ClASDBEntities db = new ClASDBEntities();
            MemberLogin obj = new MemberLogin();
            List<SelectListItem> l = new List<SelectListItem>();
            var getdata = db.RoleDetails.ToList();
            foreach (var item in getdata)
            {   if (item.RoleId == 1)
                {
                    continue;
                }
                else
                {
                    l.Add(new SelectListItem
                    {
                        Text = item.RoleName,
                        Value = item.RoleId.ToString()

                    });
                }
            }
           
            dt.ListR = l;
            if (ModelState.IsValid)
            {
                                
                    obj.EmailId = dt.Email;
                    obj.Password = dt.Password;
                    obj.RoleId = dt.RoleId;
                    
                    db.MemberLogins.Add(obj);
                    db.SaveChanges();
                    var getMemId = db.MemberLogins.Where(m => m.RoleId == dt.RoleId).OrderByDescending(o => o.MemberID).FirstOrDefault();
                    dt.MemberId = getMemId.MemberID;
                    if (dt.RoleId == 2)
                    {
                        Doctor d = new Doctor();
                        d.MemberId = getMemId.MemberID;
                        db.Doctors.Add(d);
                        db.SaveChanges();
                    }
                    else if (dt.RoleId == 3)
                    {
                        Patient p = new Patient();
                        p.MemberID = getMemId.MemberID;
                        db.Patients.Add(p);
                        db.SaveChanges();
                    }
                    else if (dt.RoleId == 4)
                    {
                        Supplier s = new Supplier();
                        s.MemberId = getMemId.MemberID;
                        db.Suppliers.Add(s);
                        db.SaveChanges();
                    }
                    ViewBag.text = "Registration successful";

                }
                else
                {
                    ViewBag.text = "Registration Unsuccessful";
                }
            
                return View(dt);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();

        }
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel cm)
        {
            int MemId = Convert.ToInt32(Session["MemberID"]);

            ClASDBEntities db = new ClASDBEntities();
            var getPass = db.MemberLogins.FirstOrDefault(m => m.MemberID == MemId);
            if (ModelState.IsValid)
            {
                if (getPass.Password == cm.OldPassword)
                {
                    
                    getPass.Password = cm.NewPassword;
                    db.SaveChanges();
                    ViewBag.text = "Password changed...";

                }
                else
                {
                    ViewBag.text = "Old Password did not match!!!";
                }
            }
            else
            {
                ViewBag.text = "Confirm new Password once again...";
            }
            return View(cm);

        }
    }
}