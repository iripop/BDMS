﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BloodForLifeDBEntities : DbContext
    {
        public BloodForLifeDBEntities()
            : base("name=BloodForLifeDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BloodCount> BloodCounts { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<DonationSite> DonationSites { get; set; }
        public virtual DbSet<Donor> Donors { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
    }
}
