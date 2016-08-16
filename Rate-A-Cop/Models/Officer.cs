using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rate_A_Cop.Models
{
    public class Officer
    {
        // properties
        public int OfficerID { get; set; }
        [MaxLength(20)]
        public string BadgeNumber { get; set; }
        [MaxLength(100)]
        public string OfficerName { get; set; }

        //navigation 
        public virtual ICollection<Review> Reviews { get; set; }
    }
}