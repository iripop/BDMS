using BDMS.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BDMS.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }

        #region Chart
        public ActionResult ChartColumn()
        {
            var _context = new BloodforLifeEntities();
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();
            var results = (from c in _context.BloodCounts select c);
            results.ToList().ForEach(rs => xValue.Add(rs.BloodType));
            results.ToList().ForEach(rs => yValue.Add(rs.Count));
            new Chart(width: 600, height: 400)
              .AddTitle("Donations by type")
              .AddSeries("Default", chartType: "Column", xValue: xValue, yValues: yValue)
              .Write("png");

            return null;
        }
        #endregion
    }
}