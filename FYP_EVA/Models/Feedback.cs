using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYP_EVA.Models
{
    public enum FeedbackStatus
    {
        Committed,
        Pending,
    }
    public class Feedback
    {
        public int FeedbackID { get; set; }

        [ForeignKey("Event")]
        public int EventID { get; set; }
        public Event Event { get; set; }

        [ForeignKey("Organiser")]
        public int OrganiserID { get; set; }
        public Organiser Organiser { get; set; }

        [ForeignKey("Volunteer")]
        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }
        public int Teamwork { get; set; }
        public int Communication { get; set; }
        public int Initiative { get; set; }
        public int Supportiveness { get; set; }
        public int Professionalism { get; set; }

        public FeedbackStatus Status { get; set; }
    }
}