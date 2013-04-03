using System;
using System.Web.Mvc;
using Tournament.Entities;

namespace Tournament.Controllers
{
    public class TeamController : BaseController
    {
        public ActionResult Index()
        {
            var teams = RavenSession.Query<Team>().Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)));
            return View(teams);
        }

        public ActionResult Detail(int id)
        {
            var model = RavenSession.Load<Team>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Team {0} does not exist", id));
            }
            return View(model);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Create(Team model)
        {
            // todo: add in the authorise token to avoid xss
            if (ModelState.IsValid)
            {
                // set the id to be the object/ to allow for incrementing id
                RavenSession.Store(model, "teams/");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = RavenSession.Load<Team>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Team {0} does not exist", id));
            }
            return View(model);
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Edit(Team model)
        {
            if (ModelState.IsValid)
            {
                RavenSession.Store(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var model = RavenSession.Load<Team>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Team {0} does not exist", id));
            }
            return View(model);
        }

        //[Authorize]
        [HttpPost]
        [ActionName("delete")]
        public ActionResult ConfirmDelete(string id)
        {
            var model = RavenSession.Load<Team>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Team {0} does not exist", id));
            }
            RavenSession.Delete(model);
            return RedirectToAction("Index");
        }
    }
}