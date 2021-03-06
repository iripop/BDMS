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
    
    public partial class Donor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Donor()
        {
            this.Donations = new HashSet<Donation>();
            this.Recipients = new HashSet<Recipient>();
        }
    
        public int DonorID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Nullable<bool> ActiveDonor { get; set; }
        public string DonorFullName { get; set; }
        public string DonorBloodType { get; set; }
        public string RhFactor { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public Nullable<double> Weight { get; set; }
        public string DonorEmail { get; set; }
        public string DonorPhoneNumber { get; set; }
        public Nullable<System.DateTime> LastScreeningDate { get; set; }
        public Nullable<bool> ArchivedDonor { get; set; }
        public Nullable<int> DonationID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Donation> Donations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recipient> Recipients { get; set; }
    }
}
