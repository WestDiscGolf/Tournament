using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.Infrastructure;
using Tournament.ViewModels;

namespace Tournament.Areas.Admin.Controllers
{
    public class ExtraController : AdminController
    {
        [HttpGet]
        [ActionName("Add")]
        public ActionResult AddToLeg(string legId, string homeTeamId, string awayTeamId)
        {
            var vm = new ExtraViewModel();
            SetupData(vm, homeTeamId, awayTeamId);
            return View("Add", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Add")]
        public ActionResult AddToLeg(ExtraViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var extra = Mapper.Map<Extra>(vm);
                extra.Player = RavenSession.Load<Player>(vm.PlayerId);
                extra.Team = RavenSession.Load<Team>(vm.TeamId);

                RavenSession.Store(extra);
                return RedirectToRoute(new { controller = "Leg", action = "Detail", id = vm.LegId });
            }
            SetupData(vm, vm.HomeTeamId, vm.AwayTeamId);
            return View("Add", vm);
        }

        [HttpGet]
        public ActionResult Delete(int id, string legId)
        {
            var extra = RavenSession.Load<Extra>(id);

            if (extra == null)
            {
                return HttpNotFound(string.Format("Extra {0} does not exist", id));
            }

            return View(Mapper.Map<ExtraViewModel>(extra));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(int id, string legId)
        {
            var extra = RavenSession.Load<Extra>(id);
            if (extra == null)
            {
                return HttpNotFound(string.Format("Extra {0} does not exist", id));
            }
            RavenSession.Delete(extra);

            return RedirectToAction("Detail", "Leg", new { id = legId });
        }

        private void SetupData(ExtraViewModel viewModel, string homeTeamId, string awayTeamId)
        {
            var players = RavenSession.Query<Player>().ToList();
            var playervm = Mapper.Map<IEnumerable<PlayerViewModel>>(players).ToList();
            var teams = RavenSession.Load<Team>(new[] { homeTeamId, awayTeamId }).ToList();
            var teamvm = Mapper.Map<IEnumerable<TeamViewModel>>(teams).ToList();

            viewModel.PlayersDataSource = new[] { new SelectListItem { Text = "", Value = "" } }
                .Union(playervm.OrderBy(p => p.FullName).Select(p => new SelectListItem { Text = p.FullName, Value = p.Id }));

            viewModel.TeamDataSource = teamvm.Select(t => new SelectListItem { Text = t.Name, Value = t.Id });

            viewModel.HomeTeamId = homeTeamId;
            viewModel.AwayTeamId = awayTeamId;
        }
    }
}