using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FYP_EVA.Models;
using static System.Net.WebRequestMethods;

namespace FYP_EVA.Controllers
{
    public class VolunteersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [ChildActionOnly]
        public ActionResult ListAllEventRelated()
        {
            string query = "SELECT * FROM Events WHERE EventID  = (SELECT Event_EventID FROM Volunteers WHERE VolunteerID = " + Session["UserID"] + ") AND EventStatus = 0 ";
            Event ev = db.Events.SqlQuery(query).SingleOrDefault();
            if (ev == null)
            {
                return null;
            }
            return PartialView(ev);
        }
        public ActionResult ParticipateAction(int? id)
        {
            Event e = db.Events.Find(id);

            //Checking for Volunteer Limit
            int limit = e.VolunteerLimit;
            if ( e.VolunteerList.Count() >= limit){
                TempData["ActionMessage"] = "Current Volunteer limit is over please select another event";
                return RedirectToAction("MainPage");
            }
            Participation newP = new Participation();
            newP.EventID = e.EventID;
            newP.Status = ParticipationStatus.Pending;
            newP.VolunteerID = Convert.ToInt32(Session["UserID"]);
            db.Participations.Add(newP);
            db.Entry(e).State = EntityState.Modified;

            db.SaveChanges();
            TempData["ActionMessage"] = "Pending participation for " + e.EventName.ToString() + " as Volunteer";
            return RedirectToAction("MainPage");
        }
        public ActionResult MainPage()
        {
            String query = "SELECT * FROM Events WHERE EventStatus = 0";
            List<Event> eventList = db.Events.SqlQuery(query).ToList();
            return View(eventList);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Volunteer user)
        {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {

                    var obj = db.Volunteers.Where(a => a.TPNumber.Equals(user.TPNumber) && a.Password.Equals(user.Password)).FirstOrDefault();
                    if (obj != null)
                    {
                        Session["UserID"] = obj.VolunteerID.ToString();
                        Session["UserName"] = obj.VolunteerName.ToString();
                        Session["UserType"] = "Volunteer";
                        Session["LoggedFlag"] = "1";
                        return RedirectToAction("MainPage", "Volunteers");
                    }
                    else
                    {
                        ViewBag.Message = "Wrong credentials ! Please make sure TP Number and Password is correct";
                        return View();
                    }
                }
        }
        public new ActionResult Profile(int? id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // GET: Volunteers
        public ActionResult Index()
        {
            return View(db.Volunteers.ToList());
        }

        // GET: Volunteers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // GET: Volunteers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Volunteers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VolunteerID,TPNumber,VolunteerName,Intake,PhoneNumber,Age,ReligionPreferences,VolunteerGender,VolunteerCountryOfOrigin,Password,Email")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Volunteers.Add(volunteer);
                db.SaveChanges();

                //Checking whether user is from Register or from Admin
                String checkSession = Convert.ToString(Session["UserType"]);
                if (checkSession == "")
                {
                    TempData["ActionMessage"] = "Successfully created profile, Please login again";
                    return RedirectToAction("Login", "Home");
                }
                return RedirectToAction("Index");
            }

            return View(volunteer);
        }

        // GET: Volunteers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VolunteerID,TPNumber,VolunteerName,Intake,PhoneNumber,Teamwork,Communication,Initiative,Supportiveness,Professionalism,Age,ReligionPreferences,VolunteerGender,VolunteerCountryOfOrigin,Password,Email")] Volunteer volunteer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(volunteer).State = EntityState.Modified;
                db.SaveChanges();
                //Check if user is Volunteer
                if (Session["UserType"].Equals("Volunteer"))
                {
                    return RedirectToAction("MainPage","Volunteers", new { id = Session["UserID"] });
                }
                return RedirectToAction("Index");
            }
            return View(volunteer);
        }

        // GET: Volunteers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return HttpNotFound();
            }
            return View(volunteer);
        }

        // POST: Volunteers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            db.Volunteers.Remove(volunteer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
