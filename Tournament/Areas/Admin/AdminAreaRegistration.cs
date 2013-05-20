using System.Web.Mvc;

namespace Tournament.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Admin"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "Admin_MainDefaultRoute",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Tournament.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                name: "Admin_RavenDbIdentifierRoute",
                url: "Admin/{controller}/{action}/{*id}",
                defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Tournament.Areas.Admin.Controllers" }
            );
        }
    }
}