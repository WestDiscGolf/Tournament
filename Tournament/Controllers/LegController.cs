using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class LegController : BaseController
    {
        public ActionResult Index(string matchId)
        {
            var model = RavenSession.Query<Leg>().ToList();
            var vm = Mapper.Map<IEnumerable<LegViewModel>>(model);
            return View(vm);
        }

        public ActionResult Detail(int id)
        {
            var leg = RavenSession.Load<Leg>(id);
            if (leg == null)
            {
                return HttpNotFound(string.Format("Leg {0} does not exist", id));
            }

            var vm = Mapper.Map<LegViewModel>(leg);
            return View(vm);
        }

        [ChildActionOnly]
        public ActionResult ByEvent(string eventId)
        {
            var legs = RavenSession.Query<Leg>().Where(x => x.EventId == eventId).ToList();
            var vm = Mapper.Map<IEnumerable<LegViewModel>>(legs);
            return PartialView("_Leg", vm);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Create(string eventId, string[] teamIds)
        {
            var vm = new LegViewModel();
            vm.EventId = eventId;
            vm.TeamIds = teamIds;

            SetupData(vm);

            return View(vm);
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LegViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var leg = Mapper.Map<Leg>(viewModel);
                leg.HomeCaptain = RavenSession.Load<Player>(viewModel.HomeCaptainId);
                leg.AwayCaptain = RavenSession.Load<Player>(viewModel.AwayCaptainId);
                leg.HomeTeam = RavenSession.Load<Team>(viewModel.HomeTeamId);
                leg.AwayTeam = RavenSession.Load<Team>(viewModel.AwayTeamId);
                RavenSession.Store(leg, "legs/");
                return RedirectToAction("Detail", "Event", new { id = viewModel.EventId.Id() });
            }

            SetupData(viewModel);
            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = RavenSession.Load<Leg>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Leg {0} does not exist", id));
            }

            var vm = Mapper.Map<LegViewModel>(model);
            SetupData(vm);

            return View(vm);
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LegViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var leg = RavenSession.Load<Leg>(viewModel.Id);
                if (leg == null)
                {
                    return HttpNotFound(string.Format("Leg {0} does not exist", viewModel.Id));    
                }

                leg.Name = viewModel.Name;
                leg.Slug = viewModel.Slug;
                leg.Date = viewModel.Date;
                leg.Location = Mapper.Map<Location>(viewModel.Location);
                leg.HomeCaptain = RavenSession.Load<Player>(viewModel.HomeCaptainId);
                leg.AwayCaptain = RavenSession.Load<Player>(viewModel.AwayCaptainId);
                leg.HomeTeam = RavenSession.Load<Team>(viewModel.HomeTeamId);
                leg.AwayTeam = RavenSession.Load<Team>(viewModel.AwayTeamId);
                RavenSession.Store(leg);
                return RedirectToAction("Detail", "Event", new { id = viewModel.EventId.Id() });
            }

            SetupData(viewModel);
            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var model = RavenSession.Load<Leg>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Leg {0} does not exist", id));
            }
            return View(Mapper.Map<LegViewModel>(model));
        }

        //[Authorize]
        [HttpPost]
        [ActionName("delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(string id)
        {
            var model = RavenSession.Load<Leg>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Leg {0} does not exist", id));
            }
            var eventId = model.EventId;
            RavenSession.Delete(model);
            return RedirectToAction("Detail", "Event", new { id = eventId.Id() });
        }

        private void SetupData(LegViewModel viewModel)
        {
            var players = RavenSession.Query<Player>().ToList();
            var playervm = Mapper.Map<IEnumerable<PlayerViewModel>>(players).ToList();
            var teams = RavenSession.Query<Team>().ToList();
            var teamvm = Mapper.Map<IEnumerable<TeamViewModel>>(teams).ToList();

            viewModel.PlayersDataSource = new[] { new SelectListItem { Text = "", Value = "" } }
                .Union(playervm.OrderBy(p => p.FullName).Select(p => new SelectListItem { Text = p.FullName, Value = p.Id }));

            viewModel.TeamDataSource = new[] { new SelectListItem { Text = "", Value = "" } }.Union(teamvm.Select(t => new SelectListItem { Text = t.Name, Value = t.Id }));
        }
    }
}
