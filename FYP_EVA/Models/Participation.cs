using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYP_EVA.Models
{
    public enum ParticipationStatus
    {
        Approved,
        Rejected,
        Pending
    }
    public class Participation
    {
        public int ParticipationID { get; set; }

        [ForeignKey("Event")]
        public int EventID { get; set; }
        public Event Event { get; set; }

        [ForeignKey("Volunteer")]
        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }

        public ParticipationStatus Status { get; set; }

        public virtual IEnumerable<Volunteer> volunteerList {get; set;}
    }
}