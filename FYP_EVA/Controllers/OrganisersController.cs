using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FYP_EVA.Models;

namespace FYP_EVA.Controllers
{
    public class OrganisersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult VolunteerList(int? id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            Event ev = db.Events.Find(id);

            IEnumerable<Participation> parList = db.Participations.Where(p => p.EventID.Equals(ev.EventID));
            
            return View(parList);
        }
        public ActionResult MainPage(int? id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            Organiser og = db.Organisers.Find(id);
            IEnumerable<Event> eventList;
            eventList = db.Events.Where(a => a.OrganiserID.Equals(og.OrganiserID));
            return View(eventList);
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Organiser user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                var obj = db.Organisers.Where(a => a.OrganiserName.Equals(user.OrganiserName) && a.Password.Equals(user.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserID"] = obj.OrganiserID.ToString();
                    Session["UserName"] = obj.OrganiserName.ToString();
                    Session["UserType"] = "Organiser";
                    Session["LoggedFlag"] = "1";
                    return RedirectToAction("MainPage", "Organisers", new { id = obj.OrganiserID});
                }
                else
                {
                    ViewBag.Message = "Wrong credentials ! Please make sure Organiser Name and Password is correct";
                    return View();
                }
            }
        }
        // GET: Organisers
        public ActionResult Index()
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            return View(db.Organisers.ToList());
        }

        // GET: Organisers/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organiser organiser = db.Organisers.Find(id);
            if (organiser == null)
            {
                return HttpNotFound();
            }
            return View(organiser);
        }

        // GET: Organisers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organisers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrganiserID,OrganiserName,OrganiserType,Password,Email")] Organiser organiser)
        {
            if (ModelState.IsValid)
            {
                organiser.Status = OrganiserStatus.Pending;
                db.Organisers.Add(organiser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(organiser);
        }

        // GET: Organisers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organiser organiser = db.Organisers.Find(id);
            if (organiser == null)
            {
                return HttpNotFound();
            }
            return View(organiser);
        }

        // POST: Organisers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrganiserID,OrganiserName,OrganiserType,Password,Status,Email")] Organiser organiser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(organiser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(organiser);
        }

        // GET: Organisers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organiser organiser = db.Organisers.Find(id);
            if (organiser == null)
            {
                return HttpNotFound();
            }
            return View(organiser);
        }

        // POST: Organisers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Organiser organiser = db.Organisers.Find(id);
            db.Organisers.Remove(organiser);
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
