using BDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BMDS.Controllers
{
    [Authorize]
    public class DonorController : Controller
    {
        #region Donors GET
        public ActionResult Donor()
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            List<string> isDonorActive = new List<string>(new string[] { "Yes", "No" });
            ViewBag.IsDonorActiveList = new SelectList(isDonorActive);

            List<string> bloodType = new List<string>(new string[] { "A", "AB", "B", "0" });
            ViewBag.BloodTypeList = new SelectList(bloodType);

            List<string> rhFactor = new List<string>(new string[] { "+(positive)", "-(negative)" });
            ViewBag.RhFactorList = new SelectList(rhFactor);

            List<DonorModel> listDonors = db.Donors.Where(x => x.ArchivedDonor == false).Select(x => new DonorModel
            {
                DonorID = x.DonorID,
                ActiveDonor = x.ActiveDonor,
                DonorFullName = x.DonorFullName,
                DonorBloodType = x.DonorBloodType,
                RhFactor = x.RhFactor,
                DateOfBirth = x.DateOfBirth,
                Weight = x.Weight,
                DonorEmail = x.DonorEmail,
                DonorPhoneNumber = x.DonorPhoneNumber,
                LastScreeningDate = x.LastScreeningDate

            }).ToList();

            ViewBag.DonorsList = listDonors;
            return View();


        }
        #endregion

        #region Donors POST
        [HttpPost]
        public ActionResult Donor(DonorModel model)
        {
            try
            {
                BloodforLifeEntities db = new BloodforLifeEntities();

                List<string> isDonorActive = new List<string>(new string[] { "Yes", "No" });
                ViewBag.IsDonorActiveList = new SelectList(isDonorActive);

                List<string> bloodType = new List<string>(new string[] { "A", "AB", "B", "0" });
                ViewBag.BloodTypeList = new SelectList(bloodType);

                List<string> rhFactor = new List<string>(new string[] { "+(positive)", "-(negative)" });
                ViewBag.RhFactorList = new SelectList(rhFactor);

                if (model.DonorID > 0)
                {
                    //Update a donor
                    Donor don = db.Donors.SingleOrDefault(x => x.DonorID == model.DonorID && x.ArchivedDonor == false);

                    don.ActiveDonor = model.ActiveDonor;
                    don.DonorFullName = model.DonorFullName;
                    don.DonorBloodType = model.DonorBloodType;
                    don.RhFactor = model.RhFactor;
                    don.DateOfBirth = model.DateOfBirth;
                    don.Weight = model.Weight;
                    don.DonorEmail = model.DonorEmail;
                    don.DonorPhoneNumber = model.DonorPhoneNumber;
                    don.LastScreeningDate = model.LastScreeningDate;

                    db.SaveChanges();

                }
                else
                {
                    //Insert a donor in database
                    Donor don = new Donor();
                    don.ActiveDonor = model.ActiveDonor;
                    don.DonorFullName = model.DonorFullName;
                    don.DonorBloodType = model.DonorBloodType;
                    don.RhFactor = model.RhFactor;
                    don.DateOfBirth = model.DateOfBirth;
                    don.Weight = model.Weight;
                    don.DonorEmail = model.DonorEmail;
                    don.DonorPhoneNumber = model.DonorPhoneNumber;
                    don.LastScreeningDate = model.LastScreeningDate; ;
                    don.ArchivedDonor = false;

                    db.Donors.Add(don);
                    db.SaveChanges();

                    int latestDonorID = don.DonorID;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        #endregion

        #region Delete donor
        public JsonResult DeleteDonor(int donorID)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            bool result = false;
            Donor don = db.Donors.SingleOrDefault(x => x.ArchivedDonor == false && x.DonorID == donorID);

            if (don != null)
            {
                don.ArchivedDonor = true;
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Donor Details
        public ActionResult ShowDonorDetail(int DonorID)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            List<DonorModel> listDon = db.Donors.Where(x => x.ArchivedDonor == false && x.DonorID == DonorID).Select(x => new DonorModel
            {
                DonorID = x.DonorID,
                ActiveDonor = x.ActiveDonor,
                DonorFullName = x.DonorFullName,
                DonorBloodType = x.DonorBloodType,
                RhFactor = x.RhFactor,
                DateOfBirth = x.DateOfBirth,
                Weight = x.Weight,
                DonorEmail = x.DonorEmail,
                DonorPhoneNumber = x.DonorPhoneNumber,
                LastScreeningDate = x.LastScreeningDate,

            }).ToList();

            ViewBag.DonorsList = listDon;

            return PartialView("_ShowDonorDetail");

        }
        #endregion

        #region Add Edit donor
        public ActionResult AddEditDonor(int DonorID)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            List<string> isDonorActive = new List<string>(new string[] { "Yes", "No" });
            ViewBag.IsDonorActiveList = new SelectList(isDonorActive);

            List<string> bloodType = new List<string>(new string[] { "A", "AB", "B", "0" });
            ViewBag.BloodTypeList = new SelectList(bloodType);

            List<string> rhFactor = new List<string>(new string[] { "+(positive)", "-(negative)" });
            ViewBag.RhFactorList = new SelectList(rhFactor);

            DonorModel model = new DonorModel();

            if (DonorID > 0)
            {
                Donor don = db.Donors.SingleOrDefault(x => x.DonorID == DonorID && x.ArchivedDonor == false);
                model.DonorID = don.DonorID;
                model.ActiveDonor = don.ActiveDonor;
                model.DonorFullName = don.DonorFullName;
                model.DonorBloodType = don.DonorBloodType;
                model.RhFactor = don.RhFactor;
                model.DateOfBirth = don.DateOfBirth;
                model.Weight = don.Weight;
                model.DonorEmail = don.DonorEmail;
                model.DonorPhoneNumber = don.DonorPhoneNumber;
                model.LastScreeningDate = don.LastScreeningDate;

            }

            return PartialView("_AddEditDonor", model);

        }
        #endregion

        #region Search
        public ActionResult GetSearchDonor(string SearchText)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();
            List<DonorModel> list = db.Donors.Where(x =>x.ArchivedDonor==false && x.DonorFullName.Contains(SearchText) ||
            x.ActiveDonor.Contains(SearchText) ||
            x.DonorBloodType.Contains(SearchText) ||
            x.RhFactor.Contains(SearchText) ||
            x.DonorEmail.Contains(SearchText)).Select(x => new DonorModel
            {
                DonorID = x.DonorID,
                ActiveDonor = x.ActiveDonor,
                DonorFullName = x.DonorFullName,
                DonorBloodType = x.DonorBloodType,
                RhFactor = x.RhFactor,
                DateOfBirth = x.DateOfBirth,
                Weight = x.Weight,
                DonorEmail = x.DonorEmail,
                DonorPhoneNumber = x.DonorPhoneNumber,
                LastScreeningDate = x.LastScreeningDate

            }).ToList();

            return PartialView("_SearchDonor", list);
        }
        #endregion
    }
}