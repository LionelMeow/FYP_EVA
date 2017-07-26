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
    public class EventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Admin & Organiser Finish Event Action
        public ActionResult FinishAction(int? id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            Event ev = db.Events.Find(id);
            ev.EventStatus = EventStatus.Completed;
            db.Entry(ev).State = EntityState.Modified;
            db.SaveChanges();

            //Find all volunteers that have the foreign key of this event
            //Remove 
            ICollection<Participation> pList = db.Participations.Where(a => a.EventID.Equals(ev.EventID)).ToList();
                
            foreach (Participation p in pList)
            {
                Feedback fb = new Feedback();
                fb.EventID = p.EventID;
                fb.VolunteerID = p.VolunteerID;
                Organiser o = db.Organisers.Where(a => a.OrganiserID.Equals(ev.OrganiserID)).FirstOrDefault();
                fb.OrganiserID = o.OrganiserID;
                fb.Initiative = 1;
                fb.Professionalism = 1;
                fb.Supportiveness = 1;
                fb.Teamwork = 1;
                fb.Communication = 1;
                fb.Status = FeedbackStatus.Pending;
                db.Feedbacks.Add(fb);
                db.SaveChanges();
            }
                     
            return RedirectToAction("Index", "Feedbacks");
        }

        // GET: Events
        public ActionResult Index()
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }

            if (Session["UserType"].Equals("Organiser"))
            {
                int id = Convert.ToInt32(Session["UserID"]);

                var personalEvents = db.Events.Where(a => a.OrganiserID.Equals(id)).Include(e => e.Organiser);
                return View(personalEvents);
            }

            var events = db.Events.Include(e => e.Organiser);
            return View(events.ToList());
        }

        // GET: Events/Details/5
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
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.OrganiserID = new SelectList(db.Organisers, "OrganiserID", "OrganiserName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventID,EventName,EventDescription,StartDateTime,EndDateTime,VolunteerLimit,OrganiserID,EventMessage")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.Organiser = db.Organisers.Find(@event.OrganiserID);
                @event.OrganiserName = @event.Organiser.OrganiserName.ToString();
                @event.EventStatus = EventStatus.Disabled;
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganiserID = new SelectList(db.Organisers, "OrganiserID", "OrganiserName", @event.OrganiserID);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganiserID = new SelectList(db.Organisers, "OrganiserID", "OrganiserName", @event.OrganiserID);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventID,EventName,EventDescription,StartDateTime,EndDateTime,VolunteerLimit,EventStatus,OrganiserID,EventMessage")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganiserID = new SelectList(db.Organisers, "OrganiserID", "OrganiserName", @event.OrganiserID);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
