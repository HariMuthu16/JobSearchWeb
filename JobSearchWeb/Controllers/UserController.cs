using JobSearchWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobSearchWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        //public ActionResult Index()
        //{
        //    return View();
        //}

        private DBJobModel db = new DBJobModel();

        public ActionResult NewUser()
        {
            return View(new UserMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser(UserMV UserMV)
        {
            if (ModelState.IsValid)
            {
                var checkuser = db.UserTables.Where(a => a.EmailAddress == UserMV.EmailAddress).FirstOrDefault();

                if (checkuser != null) 
                {
                    ModelState.AddModelError("EmailAddress", "Email is Already Registered!!");
                    return View(UserMV);
                }
               checkuser = db.UserTables.Where(a => a.UserName == UserMV.UserName).FirstOrDefault();
                if (checkuser != null)
                {
                    ModelState.AddModelError("UserName", "UserName is Already Registered!!");
                    return View(UserMV);
                }

                using(var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        var user = new UserTable();
                        user.EmailAddress = UserMV.EmailAddress;
                        user.UserName = UserMV.UserName;
                        user.Password = UserMV.Password;
                        user.ContactNo = UserMV.ContactNo;
                        user.UserTypeID = UserMV.AreYouJobProvider == true ? 2 : 3;
                        db.UserTables.Add(user);
                        db.SaveChanges();

                        if(UserMV.AreYouJobProvider == true)
                        {
                            var company = new CompanyTable();
                            company.UserID = user.UserID;

                            if (string.IsNullOrEmpty(UserMV.Company.EmailAddress))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.EmailAddress", "Company Email is Required!!");
                                return View(UserMV);
                            }
                            if (string.IsNullOrEmpty(UserMV.Company.CompanyName))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.CompanyName", "Company Name is Required!!");
                                return View(UserMV);
                            }
                            if (string.IsNullOrEmpty(UserMV.Company.Description))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.Description", "Description is Required!!");
                                return View(UserMV);
                            }
                            if (string.IsNullOrEmpty(UserMV.Company.PhoneNo))
                            {
                                trans.Rollback();
                                ModelState.AddModelError("Company.PhoneNo", "Company PhoneNo is Required!!");
                                return View(UserMV);
                            }

                            company.EmailAddress = UserMV.Company.EmailAddress;
                            company.CompanyName = UserMV.Company.CompanyName;
                            company.PhoneNo = UserMV.Company.PhoneNo;
                            company.ContactNo = UserMV.ContactNo;
                            company.Description = UserMV.Company.Description;
                            company.Logo = "~/Content/assets/img/logo/logo.png";
                            db.CompanyTables.Add(company);
                            db.SaveChanges();

                        }
                        trans.Commit();
                        return RedirectToAction("Login");    

                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Plese Provide Correct Details!");
                        trans.Rollback();
                    }

                }

            }

            return View(UserMV);
        }

        public ActionResult Login()
        {
            return View(new UserLoginMV());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLoginMV userLoginMV)
        {

            if(ModelState.IsValid)
            {
                var user = db.UserTables.Where(a => a.UserName == userLoginMV.UserName && a.Password == userLoginMV.Password).FirstOrDefault(); 
                if (user == null) 
                {
                    ModelState.AddModelError(string.Empty, "UserName or Password is Invalid!!!");
                    return View(userLoginMV);
                }
                Session["UserID"] = user.UserID;
                Session["UserName"] = user.UserName;
                Session["UserTypeID"] = user.UserTypeID;
                if(user.UserTypeID == 2)
                {
                    Session["CompanyID"] = user.CompanyTables.FirstOrDefault().CompanyID;
                }
                return RedirectToAction("Index","Home");
            }
            
            return View(userLoginMV);
        }

        public ActionResult Logout()
        {
            Session["UserID"] = string.Empty;
            Session["UserName"] = string.Empty;
            Session["UserTypeID"] = string.Empty;
            Session["CompanyID"] = string.Empty;
            return RedirectToAction("Login", "User");
        }
        public ActionResult AllUsers()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var users = db.UserTables.ToList();
            return View(users);

        }


    }
}