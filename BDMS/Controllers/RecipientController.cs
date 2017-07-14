using BDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BMDS.Controllers
{
    [Authorize]
    public class RecipientController : Controller
    {
        BloodForLifeDBEntities db = new BloodForLifeDBEntities();

        #region Recipients GET
        public ActionResult Recipients()
        {
            List<Donor> listDonor = db.Donors.Where(x => x.ActiveDonor==true && x.ArchivedDonor == false).ToList();
            ViewBag.DonorsList = new SelectList(listDonor, "DonorID", "DonorFullName");
            List<Donation> donationlist = db.Donations.Where(x => x.DonationIsArchived == false).ToList();
            ViewBag.DonationList = new SelectList(donationlist, "DonationID", "DonationType");

           

            List<RecipientModel> listRec = db.Recipients.Where(x => x.IsRecipientArchived == false).Select(x => new RecipientModel
            {
                RecipientCodedName = x.RecipientCodedName,
                DonorFullName = x.Donor.DonorFullName,
                DonationType = x.Donation.DonationType,
                DateOfUse = x.DateOfUse,
                RelatedCondition = x.RelatedCondition,
                RecipientID = x.RecipientID
            }).ToList();
            ViewBag.RecipientList = listRec;

            return View();

        }
        #endregion

        #region Recipients POST
        [HttpPost]
        public ActionResult Recipients(RecipientModel model)
        {
            try
            {
                List<Donor> listDonor = db.Donors.Where(x => x.ActiveDonor == true && x.ArchivedDonor == false).ToList();
                ViewBag.DonorsList = new SelectList(listDonor, "DonorID", "DonorFullName");
                List<Donation> donationlist = db.Donations.Where(x => x.DonationIsArchived == false).ToList();
                ViewBag.DonationList = new SelectList(donationlist, "DonationID", "DonationType");

                if (model.RecipientID > 0)
                {
                    //Update a recipient
                    Recipient rec = db.Recipients.SingleOrDefault(x => x.RecipientID == model.RecipientID && x.IsRecipientArchived == false);

                    rec.DonorID = model.DonorID;
                    rec.DonationID = model.DonationID;
                    rec.RecipientCodedName = model.RecipientCodedName;
                    rec.DateOfUse = model.DateOfUse;
                    rec.RelatedCondition = model.RelatedCondition;

                    db.SaveChanges();
                }
                else
                {
                    //Insert a recipient into database
                    Recipient rec = new Recipient();
                    rec.DateOfUse = model.DateOfUse;
                    rec.RelatedCondition = model.RelatedCondition;
                    rec.RecipientCodedName = model.RecipientCodedName;
                    rec.DonationID = model.DonationID;
                    rec.DonorID = model.DonorID;
                    rec.IsRecipientArchived = false;

                    db.Recipients.Add(rec);
                    db.SaveChanges();

                    int latestRecipientID = rec.RecipientID;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        #endregion

        #region Delete recipients
        public JsonResult DeleteRecipient(int recipientID)
        {
            bool result = false;
            Recipient rec = db.Recipients.SingleOrDefault(x => x.IsRecipientArchived == false && x.RecipientID == recipientID);

            if (rec != null)
            {
                rec.IsRecipientArchived = true;
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Recipient details
        public ActionResult ShowRecipientDetail(int RecipientID)
        {
          
            List<RecipientModel> listRec = db.Recipients.Where(x => x.IsRecipientArchived == false && x.RecipientID == RecipientID).Select(x => new RecipientModel
            {
                RecipientCodedName = x.RecipientCodedName,
                DonorFullName = x.Donor.DonorFullName,
                DonationType = x.Donation.DonationType,
                ExpirationDate = x.Donation.ExpirationDate,
                CrossBloodType = x.Donation.CrossBloodType,
                CrossRhFactor = x.Donation.CrossRhFactor,
                DateOfUse = x.DateOfUse,
                RelatedCondition = x.RelatedCondition,
                RecipientID = x.RecipientID
            }).ToList();

            ViewBag.RecipientList = listRec;

            return PartialView("_ShowRecipientDetail");

        }
        #endregion

        #region Add or edit recipient
        public ActionResult AddEditRecipient(int RecipientID)
        {
            List<Donor> listDonor = db.Donors.Where(x => x.ActiveDonor == true && x.ArchivedDonor == false).ToList();
            ViewBag.DonorsList = new SelectList(listDonor, "DonorID", "DonorFullName");
            List<Donation> donationlist = db.Donations.Where(x => x.DonationIsArchived == false).ToList();
            ViewBag.DonationList = new SelectList(donationlist, "DonationID", "DonationType");

            RecipientModel model = new RecipientModel();

            if (RecipientID > 0)
            {
                Recipient rec = db.Recipients.SingleOrDefault(x => x.RecipientID == RecipientID && x.IsRecipientArchived == false);
                model.RecipientID = rec.RecipientID;
                model.DonorID = rec.DonorID;
                model.DonationID = rec.DonationID;
                model.RecipientCodedName = rec.RecipientCodedName;
                model.DateOfUse = rec.DateOfUse;
                model.RelatedCondition = rec.RelatedCondition;

            }

            return PartialView("_AddEditRecipient", model);

        }
        #endregion

        #region Search
        public ActionResult GetSearchRecipient(string SearchText)
        {
            
            List<RecipientModel> listRec = db.Recipients.Where(x => x.IsRecipientArchived == false && (x.RecipientCodedName.Contains(SearchText) ||
            x.RelatedCondition.Contains(SearchText) ||
            x.DateOfUse.ToString().Contains(SearchText) ||
            x.Donor.DonorFullName.Contains(SearchText) ||
            x.Donation.DonationType.Contains(SearchText))).Where(x => x.IsRecipientArchived == false).Select(x => new RecipientModel
            {
                RecipientCodedName = x.RecipientCodedName,
                DonorFullName = x.Donor.DonorFullName,
                DonationType = x.Donation.DonationType,
                DateOfUse = x.DateOfUse,
                RelatedCondition = x.RelatedCondition,
                RecipientID = x.RecipientID
            }).ToList();

            return PartialView("_SearchRecipient", listRec);
        }
        #endregion
    }
}