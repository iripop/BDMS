//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BDMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Donation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Donation()
        {
            this.Recipients = new HashSet<Recipient>();
        }
    
        public int DonationID { get; set; }
        public string DonationType { get; set; }
        public string CrossBloodType { get; set; }
        public string CrossRhFactor { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public Nullable<double> NumberOfunits { get; set; }
        public string AcceptanceStatus { get; set; }
        public string ReasonsForRejection { get; set; }
        public Nullable<bool> DonationIsArchived { get; set; }
        public Nullable<int> DonorID { get; set; }
        public Nullable<int> DonationSiteID { get; set; }
        public Nullable<int> RecipientID { get; set; }
    
        public virtual DonationSite DonationSite { get; set; }
        public virtual Donor Donor { get; set; }
        public virtual Recipient Recipient { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recipient> Recipients { get; set; }
    }
}
