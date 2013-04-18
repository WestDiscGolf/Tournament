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
    public class PlayerController : BaseController
    {
        public ActionResult Test()
        {
            var results = RavenSession.Query<Result, Player_MatchResults>().ToList();

            return View(results);
        }

        public ActionResult Index()
        {
            // execute the query to allow for the IEnumerable mapping
            var players = RavenSession.Query<Player>().Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5))).ToList();

            var vm = Mapper.Map<IEnumerable<PlayerViewModel>>(players);
            return View(vm);
        }

        public ActionResult Detail(int id)
        {
            var player = RavenSession.Load<Player>(id);
            if (player == null)
            {
                return HttpNotFound(string.Format("Player {0} does not exist", id));
            }

            var vm = Mapper.Map<PlayerViewModel>(player);

            return View(vm);
        }

        [ChildActionOnly]
        public ActionResult ByTeam(string teamId)
        {
            var players = RavenSession.Query<Player>().Where(x => x.Team.Id == teamId).ToList();
            var vm = Mapper.Map<IEnumerable<PlayerViewModel>>(players);
            return PartialView("_Players", vm);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Create(string teamId)
        {
            InitialiseTeamList(teamId);
            return View();
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlayerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var player = Mapper.Map<Player>(viewModel);
                player.Team = RavenSession.Load<Team>(viewModel.TeamId);
                RavenSession.Store(player, "players/");
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = RavenSession.Load<Player>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Team {0} does not exist", id));
            }
            InitialiseTeamList();
            return View(Mapper.Map<PlayerViewModel>(model));
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlayerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = Mapper.Map<Player>(viewModel);
                model.Team = RavenSession.Load<Team>(viewModel.TeamId);
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
            var model = RavenSession.Load<Player>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Player {0} does not exist", id));
            }
            return View(Mapper.Map<PlayerViewModel>(model));
        }

        //[Authorize]
        [HttpPost]
        [ActionName("delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(string id)
        {
            var model = RavenSession.Load<Player>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Player {0} does not exist", id));
            }
            RavenSession.Delete(model);
            return RedirectToAction("Index");
        }

        private void InitialiseTeamList(string teamId = "")
        {
            var teams = RavenSession.Query<Team>().OrderBy(x => x.Name).ToList();
            ViewBag.Teams = new[] { new SelectListItem { Text = "", Value = "" } }.Union(from team in teams
                                                                                         select new SelectListItem
                                                                                         {
                                                                                             Text = team.Name,
                                                                                             Value = team.Id,
                                                                                             Selected = (string.Compare(team.Id, teamId, StringComparison.InvariantCultureIgnoreCase) == 0)
                                                                                         });
        }
    }
}