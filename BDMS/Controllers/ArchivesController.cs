using BDMS.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BMDS.Controllers
{
    [Authorize]
    public class ArchivesController : Controller
    {
        #region Blood drives
        public ActionResult DonationSitesArchive()
        {
            List<DonationSite> allCust = new List<DonationSite>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                allCust = dc.DonationSites.Where(x => x.IsSiteArchived == true).ToList();
            }
            return View(allCust);
        }

        public FileStreamResult GETPdf()
        {
            List<DonationSite> all = new List<DonationSite>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                all = dc.DonationSites.Where(x => x.IsSiteArchived == true).ToList();
            }

            WebGrid grid = new WebGrid(source: all, canPage: false, canSort: false);
            string gridHtml = grid.GetHtml(
                    columns: grid.Columns(
                        grid.Column("DonationSiteID", "ID"),
                        grid.Column("SiteName", "Name"),
                        grid.Column("StartDate", "Start date"),
                        grid.Column("EndDate", "End date"),
                        grid.Column("RegistrationEmail", "Email"),
                        grid.Column("RegistrationPhone", "Phone"),
                        grid.Column("Address", "Address"),
                        grid.Column("City", "City"),
                        grid.Column("Zip", "Zip"),
                        grid.Column("StaffingRequired", "Staff"),
                        grid.Column("MobileSite", "Drive"),
                        grid.Column("IsSiteArchived", "Archived")
                        )
                    ).ToString();

            string exportData = String.Format("<html><head>{0}</head><body>{1}</body></html>", "<style>table{ border-spacing: 10px; border-collapse: separate; }</style>", gridHtml);
            var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
            }
        }

        #endregion

        #region Donations
        public ActionResult DonationArchive()
        {
            List<Donation> allCust = new List<Donation>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                allCust = dc.Donations.Where(x => x.IsDeleted == true).ToList();
            }
            return View(allCust);
        }

        public FileStreamResult GETPdfDonations()
        {
            List<Donation> all = new List<Donation>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                all = dc.Donations.ToList();
            }

            WebGrid grid = new WebGrid(source: all, canPage: false, canSort: false);
            string gridHtml = grid.GetHtml(
                    columns: grid.Columns(
                        grid.Column("DonationID", "ID"),
                        grid.Column("DonationType", "Donation type"),
                        grid.Column("CrossBloodType", "Blood type"),
                        grid.Column("CrossRhFactor", "Rh"),
                        grid.Column("ExpirationDate", "Expiration date"),
                        grid.Column("DonationSiteID", "Donation site"),
                        grid.Column("RecipientID", "Recipient"),
                        grid.Column("DonorID", "Donor"),
                        grid.Column("CreationDate", "Creation date"),
                        grid.Column("AcceptedDonation", "Accepted donation"),
                        grid.Column("ReasonsForRejection", "Resons for not being accepted")
                        )
                    ).ToString();

            string exportData = String.Format("<html><head>{0}</head><body>{1}</body></html>", "<style>table{ border-spacing: 10px; border-collapse: separate; }</style>", gridHtml);
            var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
            }
        }

        #endregion

        #region Donors
        public ActionResult DonorArchive()
        {
            List<Donor> allCust = new List<Donor>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                allCust = dc.Donors.Where(x => x.ArchivedDonor == true).ToList();
            }
            return View(allCust);
        }

        public FileStreamResult GETPdfDonors()
        {
            List<Donor> all = new List<Donor>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                all = dc.Donors.ToList();
            }

            WebGrid grid = new WebGrid(source: all, canPage: false, canSort: false);
            string gridHtml = grid.GetHtml(
                    columns: grid.Columns(
                        grid.Column("DonorID", "ID"),
                        grid.Column("ActiveDonor", "Active donor"),
                        grid.Column("DonorFullName", "Name"),
                        grid.Column("DonorBloodType", "Blood type"),
                        grid.Column("RhFactor", "Rh"),
                        grid.Column("DateOfBirth", "Date of birth"),
                        grid.Column("Weight", "Weight"),
                        grid.Column("DonorEmail", "Email"),
                        grid.Column("DonorPhoneNumber", "Phone"),
                        grid.Column("LastScreeningDate", "Last screening date"),
                        grid.Column("ArchivedDonor", "Archived donor")
                        )
                    ).ToString();

            string exportData = String.Format("<html><head>{0}</head><body>{1}</body></html>", "<style>table{ border-spacing: 10px; border-collapse: separate; }</style>", gridHtml);
            var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
            }
        }

        #endregion

        #region Recipients
        public ActionResult RecipientsArchive()
        {
            List<Recipient> allCust = new List<Recipient>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                allCust = dc.Recipients.Where(x => x.IsRecipientArchived == true).ToList();
            }
            return View(allCust);
        }

        public FileStreamResult GETPdfRecipients()
        {
            List<Recipient> all = new List<Recipient>();
            using (BloodforLifeEntities dc = new BloodforLifeEntities())
            {
                all = dc.Recipients.ToList();
            }

            WebGrid grid = new WebGrid(source: all, canPage: false, canSort: false);
            string gridHtml = grid.GetHtml(
                    columns: grid.Columns(
                        grid.Column("RecipientID", "ID"),
                        grid.Column("DateOfUse", "Date of use"),
                        grid.Column("RelatedCondition", "Condition"),
                        grid.Column("RecipientCodedName", "Coded name"),
                        grid.Column("DonationID", "Donation"),
                        grid.Column("IsRecipientArchived", "Archived"),
                        grid.Column("DonorID", "Donor")
                        )
                    ).ToString();

            string exportData = String.Format("<html><head>{0}</head><body>{1}</body></html>", "<style>table{ border-spacing: 10px; border-collapse: separate; }</style>", gridHtml);
            var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();

                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
            }
        }

        #endregion
    }
}