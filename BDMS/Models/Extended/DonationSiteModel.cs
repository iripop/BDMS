using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class DonationSiteModel
    {
        public int DonationSiteID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "SiteName")]
        [DisplayName("Site name")]
        public string SiteName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DisplayName("Start date")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        [DisplayName("End date")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [DisplayName("Registration email")]
        public string RegistrationEmail { get; set; }

        [DisplayName("Registration phone")]
        public string RegistrationPhone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required")]
        [DisplayName("Address")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required")]
        [DisplayName("City")]
        public string City { get; set; }

        [DisplayName("Cip code")]
        public string Zip { get; set; }

        [DisplayName("Number of staff required")]
        public Nullable<int> StaffingRequired { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile site is required")]
        [DisplayName("Is blood drive?")]
        public Nullable<bool> MobileSite { get; set; }
        public bool IsSiteArchived { get; set; }
    }
}