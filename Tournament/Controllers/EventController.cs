using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.Indexes;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class EventController : BaseController
    {
        public ActionResult Index()
        {
            var model = RavenSession.Query<Event>().Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5))).OrderByDescending(x => x.Id).ToList();
            var vm = Mapper.Map<IEnumerable<EventViewModel>>(model);
            return View(vm);
        }

        public ActionResult Detail(int id)
        {
            var model = RavenSession.Load<Event>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Event {0} does not exist", id));
            }

            var vm = Mapper.Map<EventViewModel>(model);

            vm.TeamScores = new Dictionary<string, double>();

            var legs = RavenSession.Query<Leg>().Where(x => x.EventId == model.Id).ToList();
            foreach (var leg in legs)
            {
                var scores = RavenSession.Query<Leg_ScoresByTeam.Result, Leg_ScoresByTeam>().Where(x => x.LegId == leg.Id).ToList();
                foreach (var result in scores)
                {
                    if (vm.TeamScores.ContainsKey(result.TeamId))
                    {
                        vm.TeamScores[result.TeamId] += result.Total;
                    }
                    else
                    {
                        vm.TeamScores.Add(result.TeamId, result.Total);
                    }
                }
            }

            vm.Teams = Mapper.Map<IEnumerable<TeamViewModel>>(RavenSession.Load<Team>(model.TeamIds));
            return View(vm);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            InitialiseTeamList();
            return View();
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<Event>(viewModel);
                RavenSession.Store(model, "events/");
                return RedirectToAction("Index");
            }
            InitialiseTeamList();
            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = RavenSession.Load<Event>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Event {0} does not exist", id));
            }
            InitialiseTeamList();
            return View(Mapper.Map<EventViewModel>(model));
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<Event>(viewModel);
                RavenSession.Store(model);
                return RedirectToAction("Index");
            }
            InitialiseTeamList();
            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var model = RavenSession.Load<Event>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Event {0} does not exist", id));
            }
            return View(Mapper.Map<EventViewModel>(model));
        }

        //[Authorize]
        [HttpPost]
        [ActionName("delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(string id)
        {
            var model = RavenSession.Load<Event>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Event {0} does not exist", id));
            }
            RavenSession.Delete(model);
            return RedirectToAction("Index");
        }

        private void InitialiseTeamList()
        {
            var teams = RavenSession.Query<Team>().OrderBy(x => x.Name).ToList();
            ViewBag.Teams = from team in teams
                            select new SelectListItem
                                {
                                    Text = team.Name,
                                    Value = team.Id
                                };
        }
    }
}
