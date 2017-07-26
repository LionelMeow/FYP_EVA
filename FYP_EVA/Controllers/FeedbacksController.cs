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
    public class FeedbacksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Approval Action 
        public ActionResult Approve(int? id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            Feedback fb = db.Feedbacks.Find(id);
            fb.Status = FeedbackStatus.Committed;
            db.Entry(fb).State = EntityState.Modified;
            db.SaveChanges();

            Volunteer von = db.Volunteers.Find(fb.VolunteerID);
            von.Communication += fb.Communication;
            von.Initiative +=  fb.Initiative;
            von.Professionalism +=  fb.Professionalism;
            von.Supportiveness +=  fb.Supportiveness;
            von.Teamwork += fb.Teamwork;
        
            db.Entry(von).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Feedbacks
        public ActionResult Index()
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            var feedbacks = db.Feedbacks.Include(f => f.Event).Include(f => f.Organiser).Include(f => f.Volunteer);
            return View(feedbacks.ToList());
        }

        // GET: Feedbacks/Details/5
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
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName");
            ViewBag.OrganiserID = new SelectList(db.Organisers, "OrganiserID", "OrganiserName");
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "TPNumber");
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FeedbackID,EventID,OrganiserID,VolunteerID,Teamwork,Communication,Initiative,Supportiveness,Professionalism,Status")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName", feedback.EventID);
            ViewBag.OrganiserID = new SelectList(db.Organisers, "OrganiserID", "OrganiserName", feedback.OrganiserID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "TPNumber", feedback.VolunteerID);
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
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
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName", feedback.EventID);
            ViewBag.OrganiserID = new SelectList(db.Organisers, "OrganiserID", "OrganiserName", feedback.OrganiserID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "TPNumber", feedback.VolunteerID);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FeedbackID,EventID,OrganiserID,VolunteerID,Teamwork,Communication,Initiative,Supportiveness,Professionalism,Status")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventID = new SelectList(db.Events, "EventID", "EventName", feedback.EventID);
            ViewBag.OrganiserID = new SelectList(db.Organisers, "OrganiserID", "OrganiserName", feedback.OrganiserID);
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "VolunteerID", "TPNumber", feedback.VolunteerID);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
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
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["UserType"].Equals("Volunteer"))
            {
                TempData["ActionMessage"] = "You are not authorized to view this page";
                return RedirectToAction("Index", "Home");
            }
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
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
