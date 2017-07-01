using BDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMDS.Controllers
{
    public class GoogleChartController : Controller
    {
        // GET: GoogleChart
        public ActionResult Column()
        {
            return View();
        }

        public JsonResult GetSalesData()
        {
            List<Donation> sd = new List<Donation>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                sd = dc.Donations.OrderBy(a => a.CreationDate).ToList();
            }

            var chartData = new object[sd.Count + 1];
            chartData[0] = new object[]{
                    "CreationDate",
                    "DonationType",
                    "CrossBloodType",
                    "CrossRhFactor"
                };
            int j = 0;
            foreach (var i in sd)
            {
                j++;
                chartData[j] = new object[] { i.CreationDate, i.DonationType, i.CrossBloodType, i.CrossRhFactor };
            }

            return new JsonResult { Data = chartData, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}