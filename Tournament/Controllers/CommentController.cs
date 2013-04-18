using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class CommentController : BaseController
    {
        //[Authorize]
        [HttpGet]
        public ActionResult AddToMatch(string matchId)
        {
            var vm = new CommentViewModel();
            vm.AssociatedItemId = matchId;
            return View("Add", vm);
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToMatch(CommentViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var match = RavenSession.Load<Match>(vm.AssociatedItemId);
                if (match == null)
                {
                    return HttpNotFound(string.Format("Match {0} does not exist", vm.AssociatedItemId));
                }
                if (match.Comments == null)
                {
                    match.Comments = new List<Comment>();
                }

                match.Comments.Add(Mapper.Map<Comment>(vm));
                RavenSession.Store(match);
                return RedirectToRoute(new {controller = "Match", action = "Index"});
            }
            return View("Add", vm);
        }
    }
}
