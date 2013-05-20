using System.Collections.Generic;
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
                var leg = RavenSession.Load<Leg>(vm.LegId);
                if (leg == null)
                {
                    return HttpNotFound(string.Format("Leg {0} does not exist", vm.LegId));
                }
                if (leg.Extras == null)
                {
                    leg.Extras = new List<Extra>();
                }

                var extraCount = leg.Extras.Count;
                var extra = Mapper.Map<Extra>(vm);
                extra.Id = extraCount++;
                extra.Player = RavenSession.Load<Player>(vm.PlayerId);
                extra.Team = RavenSession.Load<Team>(vm.TeamId);
                leg.Extras.Add(extra);
                RavenSession.Store(leg);
                return RedirectToRoute(new { controller = "Leg", action = "Detail", id = vm.LegId });
            }
            SetupData(vm, vm.HomeTeamId, vm.AwayTeamId);
            return View("Add", vm);
        }

        [HttpGet]
        public ActionResult Delete(int id, string legId)
        {
            var leg = RavenSession.Load<Leg>(legId);
            if (leg == null)
            {
                return HttpNotFound(string.Format("Leg {0} does not exist", legId));
            }

            var extra = leg.Extras.FirstOrDefault(x => x.Id == id);
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
            var leg = RavenSession.Load<Leg>(legId);
            if (leg == null)
            {
                return HttpNotFound(string.Format("Leg {0} does not exist", legId));
            }

            var extra = leg.Extras.FirstOrDefault(x => x.Id == id);
            if (extra == null)
            {
                return HttpNotFound(string.Format("Extra {0} does not exist", id));
            }
            
            if (leg.Extras.Remove(extra))
            {
                RavenSession.Store(leg);
            }

            return RedirectToAction("Detail", "Leg", new { id = legId.Id() });
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