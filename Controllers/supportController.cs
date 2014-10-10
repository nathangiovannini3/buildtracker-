﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BuildFeed.Models.ViewModel;

namespace BuildFeed.Controllers
{
    public class supportController : Controller
    {
        // GET: support
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser ru)
        {
            if (ModelState.IsValid)
            {
                bool isAuthenticated = Membership.ValidateUser(ru.UserName, ru.Password);

                if (isAuthenticated)
                {
                    FormsAuthentication.SetAuthCookie(ru.UserName, ru.RememberMe);

                    string returnUrl = string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]) ? "/" : Request.QueryString["ReturnUrl"];

                    return Redirect(returnUrl);
                }
            }

            ViewData["ErrorMessage"] = "The username and password are not valid.";
            return View(ru);
        }

        [Authorize]
        public ActionResult Password()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Password(ChangePassword cp)
        {
            if (ModelState.IsValid)
            {
                var user = Membership.GetUser();
                bool success = user.ChangePassword(cp.OldPassword, cp.NewPassword);

                if (success)
                {
                    return Redirect("/");
                }
            }

            ViewData["ErrorMessage"] = "There was an error changing your password.";
            return View(cp);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationUser ru)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                Membership.CreateUser(ru.UserName, ru.Password, ru.EmailAddress, "THIS WILL BE IGNORED", "I WILL BE IGNORED", false, out status);

                switch (status)
                {
                    case MembershipCreateStatus.Success:
                        return RedirectToAction("thanks_register");
                    case MembershipCreateStatus.InvalidPassword:
                        ViewData["ErrorMessage"] = "The password is invalid.";
                        break;
                    case MembershipCreateStatus.DuplicateEmail:
                        ViewData["ErrorMessage"] = "A user account with this email address already exists.";
                        break;
                    case MembershipCreateStatus.DuplicateUserName:
                        ViewData["ErrorMessage"] = "A user account with this user name already exists.";
                        break;
                    default:
                        ViewData["ErrorMessage"] = "Unspecified error.";
                        break;
                }
            }

            return View(ru);
        }

        public ActionResult thanks_register()
        {
            return View();
        }
    }
}