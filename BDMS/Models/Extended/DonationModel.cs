using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class DonationModel
    {
        public int DonationID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the donation type")]
        public string DonationType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a blood type")]
        public string CrossBloodType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select the Rh factor")]
        public string CrossRhFactor { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the expiration date")]
        public System.DateTime ExpirationDate { get; set; }

        public System.DateTime CreationDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the number of units")]
        public double NumberOfUnits { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a donation site")]
        public Nullable<int> DonationSiteID { get; set; }

        public Nullable<int> RecipientID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select a donor")]
        public Nullable<int> DonorID { get; set; }



        public Nullable<bool> AcceptedDonation { get; set; }
        public string ReasonsForRejection { get; set; }
        public Nullable<bool> IsExpired { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        //Custom attributes
        public string DonorFullName { get; set; }
        public string RecipientCodedName { get; set; }
        public string SiteName { get; set; }
        public string BloodType { get; set; }
        public Nullable<int> Count { get; set; }
    }
}