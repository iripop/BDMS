using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BDMS.Models
{
    public class ChartDonationsModel
    {
        public  BloodCountModel Count { get; set; }
        public DonationModel Donation { get; set; }
    }
}