using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Tournament.Entities;
using Tournament.Infrastructure;
using Tournament.ViewModels;

namespace Tournament.Areas.Admin.Controllers
{
    public class LoginController : AdminController
    {
        [AllowAnonymous]
        public ActionResult Setup()
        {
            var original = RavenSession.Query<User>().FirstOrDefault(x => x.UserName == "Admin");
            if (original != null)
            {
                return HttpNotFound("User has already been created");
            }

            var user = new User
            {
                FirstName = "Admin",
                LastName = "User",
                UserName = "Admin",
                Enabled = true,
            }.SetPassword("tournament");

            RavenSession.Store(user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectFromLoginPage(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model, string returnUrl)
        {
            var user = RavenSession.Query<User>().FirstOrDefault(x => x.UserName == model.UserName);

            if (user == null || !user.ValidatePassword(model.Password) || !user.Enabled)
            {
                ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect or your account is not enabled.");
            }
            
            if (ModelState.IsValid)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                return RedirectFromLoginPage(returnUrl);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            var user = RavenSession.Query<User>().FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (user == null)
            {
                return HttpNotFound("User does not exist.");
            }

            return View(new ChangePasswordModel { Id = Convert.ToInt32(user.Id.Id()) });
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel viewModel)
        {
            var user = RavenSession.Load<User>(viewModel.Id);
            if (user == null)
            {
                return HttpNotFound("User does not exist.");   
            }

            if (!user.ValidatePassword(viewModel.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "Old password did not match existing password");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);   
            }

            user.SetPassword(viewModel.NewPassword);            
            return RedirectToAction("Index");
        }

        private ActionResult RedirectFromLoginPage(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Dashboard");
            return Redirect(returnUrl);
        }
    }
}