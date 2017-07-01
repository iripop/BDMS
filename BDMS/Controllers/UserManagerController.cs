using BDMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BMDS.Controllers
{
    [Authorize]
    public class UserManagerController : Controller
    {
        #region Add  a user action
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }
        #endregion

        #region Add a user POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration([Bind(Exclude = "IsEmailVerified, ActivationCode")] User userModel)
        {
            bool Status = false;
            string message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                #region Check if Email already exists
                var IsExist = IsEmailExist(userModel.EmailAddress);
                if (IsExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exists");
                    return View(userModel);
                }
                #endregion

                #region Generate activation code
                userModel.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password hashing
                userModel.Password = Crypto.Hash(userModel.Password);
                userModel.ConfirmPassword = Crypto.Hash(userModel.ConfirmPassword);
                #endregion
                userModel.IsEmailVerified = false;

                #region Save data to database
                using (BloodforLifeEntities db = new BloodforLifeEntities())
                {
                    db.Users.Add(userModel);
                    db.SaveChanges();

                    //Send email to user
                    SendVerificationLinkEmail(userModel.EmailAddress, userModel.ActivationCode.ToString(), userModel.Password);
                    message = "Registration successful. Account activation link" +
                        " has been sent to your email address:" + userModel.EmailAddress;
                    Status = true;
                }
                #endregion
            }
            else
            {
                message = "Invalid request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(userModel);


        }
        #endregion

        #region Verify account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (BloodforLifeEntities db = new BloodforLifeEntities())
            {
                db.Configuration.ValidateOnSaveEnabled = false; // added to avoid confirm password does 
                                                                //not match issue on save changes
                var v = db.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVerified = true;
                    db.SaveChanges();
                    Status = true;
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            ViewBag.Status = Status;
            return View();
        }
        #endregion

        [NonAction]
        public bool IsEmailExist(string userEmail)
        {
            using (BloodforLifeEntities db = new BloodforLifeEntities())
            {
                var v = db.Users.Where(a => a.EmailAddress == userEmail).FirstOrDefault();
                return v != null;
            }
        }

        [NonAction]
        protected void SendVerificationLinkEmail(string userEmail, string activationCode, string password)
        {
            var verifyUrl = "/UserManager/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("pop.irina1@gmail.com", "Blood donations management system");
            var toEmail = new MailAddress(userEmail);
            var fromEmailPassword = "Frunzulitza.1"; //Replace with actual password
            string subject = "Your account is succesfully created";
            string body = "<br/><b/r>We are excited to tell you that your Blood donations management system account is" +
                " successfully created.Your current password is:" + password + " Please click on the link below to verify your account" +
                "<b/r><br/> <a href='" + link + "'>" + link + "</a> ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)

            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        #region Change password get action
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        #endregion

       /*
        #region Change pasword POST action
        [HttpPost]
        public ActionResult ChangePassword(User userModel)
        {
            bool Status = false;
            string message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                #region Check if passwords match
                var IsExist = IsPasswordCorrect(userModel.Password);
                if (IsExist)
                {
                    ModelState.AddModelError("PasswordExist", "Current password incorrect");
                    return View(userModel);
                }
                #endregion

                #region Password hashing
                userModel.NewPassword = Crypto.Hash(userModel.NewPassword);
                userModel.ConfirmPassword = Crypto.Hash(userModel.ConfirmPassword);
                #endregion

                #region Save data to database
                try
                {
                    BloodDonorDBEntities db = new BloodDonorDBEntities();
                    if (userModel.U > 0)
                    {
                        //Update a donor
                        User user = db.Users.SingleOrDefault(x => x.UserID == userModel.UserID);

                        user.Password = userModel.Password;

                        db.SaveChanges();

                    }
                    return View(userModel);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }

                #endregion
            }
            else
            {
                message = "Invalid request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(userModel);


        }
        #endregion
    */

        [NonAction]
        public bool IsPasswordCorrect(string password)
        {
            using (BloodforLifeEntities db = new BloodforLifeEntities())
            {
                var v = db.Users.Where(a => a.Password == password).FirstOrDefault();
                return v != null;
            }
        }

        #region Users GET
        public ActionResult Users()
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            List<UserModel> listUsers = db.Users.Select(x => new UserModel
            {
                UserID = x.UserID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber,
                IsUserAdmin = x.IsUserAdmin

            }).ToList();
            ViewBag.UsersList = listUsers;

            return View();

        }
        #endregion

        #region Users POST
        [HttpPost]
        public ActionResult Users(User model)
        {
            try
            {
                BloodforLifeEntities db = new BloodforLifeEntities();

                if (model.UserID > 0)
                {
                    //Update a recipient
                    User u = db.Users.SingleOrDefault(x => x.UserID == model.UserID);

                    u.UserID = model.UserID;
                    u.FirstName = model.FirstName;
                    u.LastName = model.LastName;
                    u.EmailAddress = model.EmailAddress;
                    u.PhoneNumber = model.PhoneNumber;
                    u.IsUserAdmin = model.IsUserAdmin;

                    db.SaveChanges();
                }
                else
                {

                }

                return View(model);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }
        #endregion

        #region Delete user
        public JsonResult DeleteUser(int userID)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();

            bool result = false;
            User rec = db.Users.SingleOrDefault(x => x.UserID == userID);

            if (rec != null)
            {
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Search
        public ActionResult GetSearchUser(string SearchText)
        {
            BloodforLifeEntities db = new BloodforLifeEntities();


            List<UserModel> listUsers = db.Users.Where(x => x.FirstName.Contains(SearchText) ||
            x.LastName.Contains(SearchText) ||
            x.EmailAddress.ToString().Contains(SearchText) ||
            x.PhoneNumber.Contains(SearchText) ||
            x.IsUserAdmin.ToString().Contains(SearchText)).Select(x => new UserModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmailAddress = x.EmailAddress,
                PhoneNumber = x.PhoneNumber,
                IsUserAdmin = x.IsUserAdmin,
            }).ToList();

            return PartialView("_SearchUser", listUsers);
        }
        #endregion
    }
}