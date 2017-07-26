using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYP_EVA.Models
{
    public enum EventStatus
    {
        Recruiting, //0 
        Disabled, //1
        Completed //2
    }
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public String EventName { get; set; }

        [Display(Name = "Event Description")]
        [DataType(DataType.MultilineText)]
        public String EventDescription { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Event Start Date")]
        public DateTime StartDateTime { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        [Display(Name = "Event End Date")]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "Volunteer Limit")]
        public int VolunteerLimit { get; set; }

        [Display(Name = "Event Status")]
        public EventStatus EventStatus { get; set; }

        [ForeignKey("Organiser")]
        [Display(Name = "Organiser Assiociated")]
        public int OrganiserID { get; set; }
        public virtual Organiser Organiser { get; set; }
        public String OrganiserName { get; set; }
        public virtual ICollection<Volunteer> VolunteerList { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Event Message")]
        public String EventMessage { get; set; }
    }
}