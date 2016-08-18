using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Rate_A_Cop.Models
{
    public class Review
    {
        // properties
        public int ReviewID { get; set; }
        public bool IsAnonymous { get; set; }
        public string ReviewText { get; set; }
        public short ReviewType { get; set; }
        public string Location { get; set; }
        [DataType(DataType.DateTime)]
        public string ReviewTimeStamp { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ReviewDateTime { get; set; }

        // navigation properties
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Officer Officer { get; set; }
    }
}