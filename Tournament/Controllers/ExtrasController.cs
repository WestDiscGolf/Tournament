using System.Linq;
using System.Web.Mvc;
using Tournament.Entities;
using Tournament.Infrastructure.Indexes;

namespace Tournament.Controllers
{
    public class ExtraController : BaseController
    {
        [ChildActionOnly]
        public ActionResult ByPlayer(string playerId)
        {
            var extras = RavenSession.Query<Extra, Extras_ByPlayer>().Where(x => x.PlayerId == playerId).ToList();
            return PartialView("_ExtraByPlayer", extras);
        }
    }
}
