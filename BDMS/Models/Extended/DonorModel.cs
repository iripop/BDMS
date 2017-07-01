using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class DonorModel
    {
        public int DonorID { get; set; }
        public string ActiveDonor { get; set; }
        public string DonorFullName { get; set; }
        public string DonorBloodType { get; set; }
        public string RhFactor { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<double> Weight { get; set; }
        public string DonorEmail { get; set; }
        public string DonorPhoneNumber { get; set; }
        public Nullable<System.DateTime> LastScreeningDate { get; set; }
        public Nullable<bool> ArchivedDonor { get; set; }

        //Custom attributes
        public string DonationType { get; set; }
        public string CrossBloodType { get; set; }
        public string CrossRhFactor { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public double NumberOfUnits { get; set; }
    }
}