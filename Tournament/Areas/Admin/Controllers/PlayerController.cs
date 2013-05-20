using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.ViewModels;

namespace Tournament.Areas.Admin.Controllers
{
    public class PlayerController : AdminController
    {
        public ActionResult Index()
        {
            // execute the query to allow for the IEnumerable mapping
            var players = RavenSession.Query<Player>().Customize(x => x.WaitForNonStaleResultsAsOfNow()).ToList();
            var vm = Mapper.Map<IEnumerable<PlayerViewModel>>(players);
            return View(vm);
        }

        [HttpGet]
        public ActionResult Create(string teamId)
        {
            InitialiseTeamList(teamId);
            return View();
        }

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

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = RavenSession.Load<Player>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Player {0} does not exist", id));
            }
            InitialiseTeamList();
            return View(Mapper.Map<PlayerViewModel>(model));
        }

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