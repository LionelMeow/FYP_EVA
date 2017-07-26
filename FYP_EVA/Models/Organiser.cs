using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYP_EVA.Models
{
    public enum OrganiserType
    {
        Clubs,
        Societies,
        Company
    }

    public enum OrganiserStatus
    {
        Accepted,
        Rejected,
        Pending,
    }
    public class Organiser
    {
        public int OrganiserID { get; set; }

        [Required]
        [Display(Name = "Organiser Name")] //Register with Name 
        public String OrganiserName { get; set; }

        [Display(Name = "Organiser Type")]
        public OrganiserType OrganiserType { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }
        public OrganiserStatus Status { get; set; }

        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
    }


}