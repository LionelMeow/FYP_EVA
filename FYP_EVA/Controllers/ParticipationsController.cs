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
    public class ParticipationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult ApproveAction(int? id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            Participation participation = db.Participations.Find(id);
            //Checking for Volunteer Limit
            Event e = db.Events.Find(participation.EventID);
            int limit = e.VolunteerLimit;

            if (e.VolunteerList.Count() >= limit)
            {
                TempData["ActionMessage"] = "Current Volunteer Limit is reached cannot deploy more volunteers";
                return RedirectToAction("MainPage");
            }

            participation.Status = ParticipationStatus.Approved;
            db.Entry(participation).State = EntityState.Modified;
            db.SaveChanges();

            Volunteer v = db.Volunteers.Find(participation.VolunteerID);
            e.VolunteerList.Add(v);
            db.Entry(e).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Participations
        public ActionResult Index()
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            var participations = db.Participations.Include(p => p.Event).Include(p => p.Volunteer);
            return View(participations.ToList());
        }

        // GET: Participations/Details/5
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
            Participation participation = db.Participations.Find(id);
            if (participation == null)
            {
                return HttpNotFound();
            }
            return View(participation);
        }

        // GET: Participations/Create
        public ActionResult Create()
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName");
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "TPNumber");
            return View();
        }

        // POST: Participations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ParticipationID,EventID,VolunteerID")] Participation participation)
        {
            if (ModelState.IsValid)
            {
                participation.Status = ParticipationStatus.Pending;
                db.Participations.Add(participation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName", participation.EventID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "TPNumber", participation.VolunteerID);
            return View(participation);
        }

        // GET: Participations/Edit/5
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
            Participation participation = db.Participations.Find(id);
            if (participation == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName", participation.EventID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "TPNumber", participation.VolunteerID);
            return View(participation);
        }

        // POST: Participations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ParticipationID,EventID,VolunteerID,Status")] Participation participation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(participation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName", participation.EventID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "TPNumber", participation.VolunteerID);
            return View(participation);
        }

        // GET: Participations/Delete/5
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
            Participation participation = db.Participations.Find(id);
            if (participation == null)
            {
                return HttpNotFound();
            }
            return View(participation);
        }

        // POST: Participations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Participation participation = db.Participations.Find(id);
            db.Participations.Remove(participation);
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
