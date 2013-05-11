using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tournament.Areas.Admin.Controllers
{
    public class DashboardController : AdminController
    {
        //
        // GET: /Admin/Dashboard/

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return Content("Logged In: " + User.Identity.Name);    
            }
            return Content("If we've got here, then big problems");
        }
    }
}