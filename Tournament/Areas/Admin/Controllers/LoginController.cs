using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Tournament.Entities;
using Tournament.ViewModels;

namespace Tournament.Areas.Admin.Controllers
{
    public class LoginController : AdminController
    {
        [AllowAnonymous]
        public ActionResult Setup()
        {
            var user = new User
            {
                FirstName = "Adam",
                UserName = "West",
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
                return RedirectFromLoginPage();
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

        private ActionResult RedirectFromLoginPage(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Dashboard");
            return Redirect(returnUrl);
        }
    }
}