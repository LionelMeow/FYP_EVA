using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYP_EVA.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public enum CountryOfOrigin
    {
        Malaysian,
        NonMalaysian
    }

    public enum Religion
    {
        Islam,
        Christian,
        Hinduism,
        Buddhist,
        Freethinker
    }

    public class Volunteer
    {
        public int VolunteerID { get; set; }

        [Required]
        [Display(Name = "TP Number")]
        public String TPNumber { get; set; }

        [Display(Name = "Volunteer Name")]
        public String VolunteerName { get; set; }

        [Display(Name = "Intake Code")]
        public String Intake { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public String PhoneNumber { get; set; }

        //Attributes - Should Be Hidden 
        public int Teamwork { get; set; }
        public int Communication { get; set; }
        public int Initiative { get; set; }
        public int Supportiveness { get; set; }
        public int Professionalism { get; set; }

        [Range(16, 99, ErrorMessage = "Age should be between 18 and 99")]
        public int Age { get; set; }

        [Display(Name = "Religion Preferences")]
        public Religion ReligionPreferences { get; set; }

        [Display(Name = "Gender of Volunteer")]
        public Gender VolunteerGender { get; set; }

        [Display(Name = "Country Of Origin")]
        public CountryOfOrigin VolunteerCountryOfOrigin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }
    }
}