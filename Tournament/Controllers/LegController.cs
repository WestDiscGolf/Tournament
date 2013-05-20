using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.Infrastructure.Indexes;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class LegController : BaseController
    {
        public ActionResult Test()
        {
            var results = RavenSession.Query<Leg_ScoresByTeam.Result, Leg_ScoresByTeam>().ToList();

            return View(results);
        }

        public ActionResult Index()
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

            vm.TeamScores = new Dictionary<string, double>();

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

            return View(vm);
        }

        [ChildActionOnly]
        public ActionResult ByEvent(string eventId)
        {
            var legs = RavenSession.Query<Leg>().Where(x => x.EventId == eventId).ToList();
            var vm = Mapper.Map<IEnumerable<LegViewModel>>(legs);
            return PartialView("_Leg", vm);
        }
    }
}
