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
    public class UserController : Controller
    {

        #region Login GET
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        #endregion

        #region Login POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login, string ReturnUrl = "")
        {
            string message = "";
            using (BloodForLifeDBEntities db = new BloodForLifeDBEntities())
            {
                var v = db.Users.Where(a => a.EmailAddress == login.EmailAddress).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                    {
                        int timeout = login.RememberMe ? 525600 : 20; //525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(login.EmailAddress, login.RememberMe, timeout);
                        string encrypt = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypt);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("DonationsByExpirationDate", "Dashboard");
                        }

                    }
                    else
                    {
                        message = "Invalid credentials provided";
                    }
                }
                else
                {
                    message = "Invalid credentials provided";
                }
            }
            ViewBag.Message = message;
            return View();
        }
        #endregion

        #region LogOut
        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User"); //redirect to action Login in User controller
        }
        #endregion

        #region Forgot password get action
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        #endregion

        #region Forgot password post action
        [HttpPost]
        public ActionResult ForgotPassword([Bind(Exclude = "IsEmailVerified, ActivationCode")] ForgotPasswordModel model)
        {
            bool Status = false;
            string message = "";
            if (ModelState.IsValid)
            {
                #region Generate activation code
                model.ActivationCode = Guid.NewGuid();
                #endregion

                #region Password generate
                model.Password = Membership.GeneratePassword(12, 1);
                #endregion
                model.IsEmailVerified = false;

                #region Save data to DB
                using (BloodForLifeDBEntities db = new BloodForLifeDBEntities())
                {
                    var v = db.Users.Where(a => a.EmailAddress == model.EmailAddress).FirstOrDefault();
                    if (v != null)
                    {
                        if (string.Compare(model.EmailAddress, v.EmailAddress) == 0)
                        {
                            User user = db.Users.SingleOrDefault(x => x.UserID == model.UserID);
                            user.Password = model.Password;
                            user.ActivationCode = model.ActivationCode;
                           // user.IsEmailVerified = model.IsEmailVerified;
                            db.SaveChanges();
                            //Send email to user
                            SendVerificationLinkEmailForPasswordReset(model.EmailAddress, model.ActivationCode.ToString(), model.Password);
                            message = "Request successful. Account activation link" +
                                " has been sent to your email address:" + model.EmailAddress;
                            Status = true;
                        }
                        else
                        {
                            message = "Invalid email provided";
                        }
                    }
                    else
                    {
                        message = "Invalid email provided";
                    }
                }
                #endregion

            }
            else
            {
                message = "Invalid request";
            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(model);
        }

        #endregion

        #region Verify account
        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            bool Status = false;
            using (BloodForLifeDBEntities db = new BloodForLifeDBEntities())
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
        protected void SendVerificationLinkEmailForPasswordReset(string userEmail, string activationCode, string password)
        {
            var verifyUrl = "/User/VerifyAccount/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("pop.irina1@gmail.com", "Blood donations management system");
            var toEmail = new MailAddress(userEmail);
            var fromEmailPassword = "Frunzulitza.1"; //Replace with actual password
            string subject = "Your password was succesfully reset";
            string body = "<br/><b/r>Please click on the link below to activate the new account and login" +
                "Your current password is:" + password + " After login, please go to settings and change your password" +
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
    }
}