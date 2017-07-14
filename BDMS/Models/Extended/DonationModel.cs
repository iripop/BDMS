using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class DonationModel
    {
        public int DonationID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the donation type")]
        [DisplayName("Type of donation")]
        public string DonationType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a blood type")]
        [DisplayName("Cross blood type")]
        public string CrossBloodType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select the Rh factor")]
        [DisplayName("Cross Rh factor")]
        public string CrossRhFactor { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the number of units")]
        [DisplayName("Number of units")]
        public Nullable<double> NumberOfunits { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a donation site")]
        [DisplayName("Donation site")]
        public Nullable<int> DonationSiteID { get; set; }

        [DisplayName("Recipient")]
        public Nullable<int> RecipientID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a donor")]
        [DisplayName("Donor")]
        public Nullable<int> DonorID { get; set; }

        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string AcceptanceStatus { get; set; }
        public string ReasonsForRejection { get; set; }
        public Nullable<bool> DonationIsArchived { get; set; }

        //Custom attributes
        public string DonorFullName { get; set; }
        public string RecipientCodedName { get; set; }
        public string SiteName { get; set; }
        public string BloodType { get; set; }
        public Nullable<int> Count { get; set; }
    }
}