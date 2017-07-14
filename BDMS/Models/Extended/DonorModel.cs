using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class DonorModel
    {
        public int DonorID { get; set; }

        [DisplayName("Active donor")]
        public Nullable<bool> ActiveDonor { get; set; }
        [DisplayName("Donor name")]
        public string DonorFullName { get; set; }
        [DisplayName("Donor blood type")]
        public string DonorBloodType { get; set; }
        [DisplayName("Donor Rh factor")]
        public string RhFactor { get; set; }
        [DisplayName("Donor date of birth")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        [DisplayName("Donor weight")]
        public Nullable<double> Weight { get; set; }
        [DisplayName("Donor email address")]
        public string DonorEmail { get; set; }
        [DisplayName("Donor phone")]
        public string DonorPhoneNumber { get; set; }
        [DisplayName("Last Screening date")]
        public Nullable<System.DateTime> LastScreeningDate { get; set; }
        [DisplayName("Archived Donor?")]
        public Nullable<bool> ArchivedDonor { get; set; }

        //Custom attributes
        public string DonationType { get; set; }
        public string CrossBloodType { get; set; }
        public string CrossRhFactor { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public double NumberOfUnits { get; set; }
    }
}