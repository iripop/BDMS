using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class BloodCountModel
    {
        public int BloodTypeID { get; set; }
        public string BloodType { get; set; }
        public Nullable<int> Count { get; set; }
    }
}