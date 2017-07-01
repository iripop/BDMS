using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class DonationSiteModel
    {
        public int DonationSiteID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "SiteName")]
        public string SiteName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> EndDate { get; set; }

        public string RegistrationEmail { get; set; }
        public string RegistrationPhone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required")]
        public string City { get; set; }

        public string Zip { get; set; }
        public Nullable<int> StaffingRequired { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mobile site is required")]
        public bool MobileSite { get; set; }
        public bool IsSiteArchived { get; set; }
    }
}