using BDMS.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BMDS.Controllers
{
    [Authorize]
    public class ArchivesController : Controller
    {
        BloodForLifeDBEntities db = new BloodForLifeDBEntities();

        #region Blood drives
        public ActionResult DonationSitesArchive(int page = 1)
        {
            int pageSize = 10;
            int totalPage = 0;
            int totalRecord = 0;
            List<DonationSite> allCust = new List<DonationSite>();
            using (BloodForLifeDBEntities dc = new BloodForLifeDBEntities())
            {
                totalRecord = dc.Recipients.Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                allCust = dc.DonationSites.Take(pageSize).Where(x => x.IsSiteArchived == true).ToList();
            }
            ViewBag.TotalRows = totalRecord;
            ViewBag.PageSize = pageSize;
            return View(allCust);
        }

        public FileStreamResult GETPdf()
        {
            List<DonationSite> all = new List<DonationSite>();
            using (BloodForLifeDBEntities dc = new BloodForLifeDBEntities())
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

        public void GETExcel()
        {
            List<DonationSite> allDonSites = new List<DonationSite>();
            using (BloodForLifeDBEntities db = new BloodForLifeDBEntities())
            {
                allDonSites = db.DonationSites.Where(x => x.IsSiteArchived == true).ToList();
            }
            WebGrid grid = new WebGrid(source: allDonSites, canPage: false, canSort: false);
            string gridData = @grid.GetHtml(
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
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=DonationSiteArchive.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();

        }

        #endregion

        #region Donations
        public ActionResult DonationArchive(int page = 1)
        {
            int pageSize = 10;
            int totalPage = 0;
            int totalRecord = 0;
            List<Donation> allCust = new List<Donation>();
            using (BloodForLifeDBEntities dc = new BloodForLifeDBEntities())
            {
                totalRecord = dc.Recipients.Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                allCust = dc.Donations.Take(pageSize).Where(x => x.DonationIsArchived == true).ToList();
            }
            ViewBag.TotalRows = totalRecord;
            ViewBag.PageSize = pageSize;
            return View(allCust);
        }

        public FileStreamResult GETPdfDonations()
        {
            List<Donation> all = new List<Donation>();
            using (BloodForLifeDBEntities dc = new BloodForLifeDBEntities())
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
                        grid.Column("AcceptanceStatus", "Accepted donation"),
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

        public void GETExcelDonations()
        {
            List<Donation> allDon = new List<Donation>();
            using (BloodForLifeDBEntities db = new BloodForLifeDBEntities())
            {
                allDon = db.Donations.Where(x => x.DonationIsArchived == true).ToList();
            }
            WebGrid grid = new WebGrid(source: allDon, canPage: false, canSort: false);
            string gridData = @grid.GetHtml(
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
                        grid.Column("AcceptanceStatus", "Accepted donation"),
                        grid.Column("ReasonsForRejection", "Resons for not being accepted")
                    )
                ).ToString();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=DonationsArchive.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();

        }

        #endregion

        #region Donors
        public ActionResult DonorArchive(int page = 1)
        {
            int pageSize = 10;
            int totalPage = 0;
            int totalRecord = 0;
            List<Donor> allCust = new List<Donor>();
            using (BloodForLifeDBEntities dc = new BloodForLifeDBEntities())
            {
                totalRecord = dc.Recipients.Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                allCust = dc.Donors.Take(pageSize).Where(x => x.ArchivedDonor == true).ToList();
            }
            ViewBag.TotalRows = totalRecord;
            ViewBag.PageSize = pageSize;
            return View(allCust);
        }

        public FileStreamResult GETPdfDonors()
        {
            List<Donor> all = new List<Donor>();
            using (BloodForLifeDBEntities dc = new BloodForLifeDBEntities())
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

        public void GETExcelDonors()
        {
            List<Donor> allDonors = new List<Donor>();
            using (BloodForLifeDBEntities db = new BloodForLifeDBEntities())
            {
                allDonors = db.Donors.Where(x => x.ArchivedDonor == true).ToList();
            }
            WebGrid grid = new WebGrid(source: allDonors, canPage: false, canSort: false);
            string gridData = @grid.GetHtml(
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
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=DonorsArchive.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();

        }

        #endregion

        #region Recipients
        public ActionResult RecipientsArchive(int page=1)
        {
            int pageSize = 10;
            int totalPage = 0;
            int totalRecord = 0;
            List<Recipient> allCust = new List<Recipient>();
            using (BloodForLifeDBEntities dc = new BloodForLifeDBEntities())
            {
                totalRecord = dc.Recipients.Where(x => x.IsRecipientArchived == true).Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                allCust = dc.Recipients.Take(pageSize).Where(x => x.IsRecipientArchived == true).ToList();
            }
            ViewBag.TotalRows = totalRecord;
            ViewBag.PageSize = pageSize;
            return View(allCust);
        }

        public FileStreamResult GETPdfRecipients()
        {
            List<Recipient> all = new List<Recipient>();
            using (BloodForLifeDBEntities dc = new BloodForLifeDBEntities())
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

        public void GETExcelRecipients()
        {
            List<Recipient> allRec = new List<Recipient>();
            using (BloodForLifeDBEntities db = new BloodForLifeDBEntities())
            {
                allRec = db.Recipients.Where(x => x.IsRecipientArchived == true).ToList();
            }
            WebGrid grid = new WebGrid(source: allRec, canPage: false, canSort: false);
            string gridData = @grid.GetHtml(
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
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=RecipientsArchive.xls");
            Response.ContentType = "application/excel";
            Response.Write(gridData);
            Response.End();

        }

        #endregion
    }
}