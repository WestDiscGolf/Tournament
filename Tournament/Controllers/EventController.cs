using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.Infrastructure.Indexes;
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
    }
}