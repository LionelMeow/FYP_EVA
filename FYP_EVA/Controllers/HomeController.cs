using FYP_EVA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FYP_EVA.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Admin(User admin)
        {
            if ((admin.UserID == "adminAPU") && (admin.Password == "Password123"))
            {
                Session["UserID"] = admin.UserID;
                Session["UserType"] = "Admin";
                Session["LoggedFlag"] = "1";
                Session["UserName"] = "EVA Admin";
                return RedirectToAction("MainPage");
            }
            else
            {
                ViewBag.Message = "Wrong credentials ! Please make sure that both ID and password are correct";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }

        public ActionResult MainPage()
        {
            if (Session["UserType"].ToString() != "Admin".ToString())
            {
                TempData["ActionMessage"] = "Only Admin is allowed for this page";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [ChildActionOnly]
        public ActionResult ViewBestCommunication()
        {   
     
            string query = "SELECT * FROM Volunteers WHERE Communication = (SELECT MAX(Communication) FROM Volunteers)";
            Volunteer von = db.Volunteers.SqlQuery(query).SingleOrDefault();
            return PartialView(von);
        }

        [ChildActionOnly]
        public ActionResult ViewBestInitiative()
        {

            string query = "SELECT * FROM Volunteers WHERE Initiative = (SELECT MAX(Initiative) FROM Volunteers)";
            Volunteer von = db.Volunteers.SqlQuery(query).SingleOrDefault();
            return PartialView(von);
        }

        [ChildActionOnly]
        public ActionResult ViewBestSupportiveness()
        {

            string query = "SELECT * FROM Volunteers WHERE Supportiveness = (SELECT MAX(Supportiveness) FROM Volunteers)";
            Volunteer von = db.Volunteers.SqlQuery(query).SingleOrDefault();
            return PartialView(von);
        }

        [ChildActionOnly]
        public ActionResult ViewBestProfessionalism()
        {

            string query = "SELECT * FROM Volunteers WHERE Professionalism = (SELECT MAX(Professionalism) FROM Volunteers)";
            Volunteer von = db.Volunteers.SqlQuery(query).SingleOrDefault();
            return PartialView(von);
        }


        [ChildActionOnly]
        public ActionResult ViewBestTeamwork()
        {

            string query = "SELECT * FROM Volunteers WHERE Teamwork = (SELECT MAX(Teamwork) FROM Volunteers)";
            Volunteer von = db.Volunteers.SqlQuery(query).SingleOrDefault();
            return PartialView(von);
        }
    }
}