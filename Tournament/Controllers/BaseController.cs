using System.Web.Mvc;
using Raven.Client;

namespace Tournament.Controllers
{
    public class BaseController : Controller
    {
        public IDocumentSession RavenSession { get; protected set; }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RavenSession = (IDocumentSession)HttpContext.Items[MvcApplication.CurrentRequestRavenSessionKey];
        }
    }
}