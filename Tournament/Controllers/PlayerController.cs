using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class PlayerController : BaseController
    {
        public ActionResult Index()
        {
            // execute the query to allow for the IEnumerable select execution below
            var players = RavenSession.Query<Player>().Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5))).ToList();

            var vm = players.Select(Mapper.Map<PlayerViewModel>).ToList();
            
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

        //[Authorize]
        [HttpGet]
        public ActionResult Create(string teamId)
        {
            InitialiseTeamList(teamId);
            return View();
        }

        //[Authorize]
        [HttpPost]
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