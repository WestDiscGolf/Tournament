using System.Web.Mvc;
using Tournament.Controllers;

namespace Tournament.Areas.Admin.Controllers
{
    [Authorize]
    public abstract class AdminController : BaseController
    {

    }
}