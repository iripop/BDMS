using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class RecipientModel
    {
        public int RecipientID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Date of use is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public Nullable<System.DateTime> DateOfUse { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Related condition is required")]
        public string RelatedCondition { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Recipient is required")]
        public string RecipientCodedName { get; set; }
        public bool IsRecipientArchived { get; set; }

        public int DonorID { get; set; }
        public int DonationID { get; set; }

        //Custom attributes
        public string CrossBloodType { get; set; }
        public string CrossRhFactor { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public string DonorFullName { get; set; }
        public string DonationType { get; set; }
        public bool IsDeleted { get; set; }
    }
}