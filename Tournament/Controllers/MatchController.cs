using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.Enumerations;
using Tournament.Infrastructure.Indexes;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class MatchController : BaseController
    {
        /// <summary>
        /// Returns all the matches, can be restricted by leg, player or team ... maybe?!
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string legId = "", string playerId = "", string teamId = "")
        {
            //string result = string.Format("legid = {0}, playerId = {1}, team = {2}", legId, playerId, teamId);

            var model = RavenSession.Query<Match>().Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5))).ToList();

            var vm = Mapper.Map<IEnumerable<MatchViewModel>>(model);

            return View(vm);
        }

        [ChildActionOnly]
        public ActionResult ByLeg(string legId)
        {
            var model = RavenSession.Query<Match>().Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5))).Where(x => x.LegId == legId).ToList();

            var vm = Mapper.Map<IEnumerable<MatchViewModel>>(model);

            return PartialView("_Match", vm);
        }

        public ActionResult ByPlayer(string playerId)
        {
            var model = RavenSession.Query<Match_ByPlayer.Result, Match_ByPlayer>().Where(x => x.PlayerId == playerId).ToList();

            var matches = RavenSession.Load<Match>(model.Select(x => x.MatchId)).ToArray();
            var vm = Mapper.Map<IEnumerable<MatchViewModel>>(matches);

            return PartialView("_Match", vm);
        }

        public ActionResult Detail(int id)
        {
            var match = RavenSession.Load<Match>(id);
            if (match == null)
            {
                return HttpNotFound(string.Format("Match {0} does not exist", id));
            }

            var vm = Mapper.Map<MatchViewModel>(match);

            return View(vm);
        }
    }
}