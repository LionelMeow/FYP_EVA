using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYP_EVA.Models
{
    public class User
    {
        // Create for Admin
        public String UserID { get; set; }


        [DataType(DataType.Password)]
        public String Password { get; set; }
    }
}