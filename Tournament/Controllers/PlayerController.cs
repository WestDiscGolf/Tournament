using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.Infrastructure.Indexes;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class PlayerController : BaseController
    {
        public ActionResult Index()
        {
            // execute the query to allow for the IEnumerable mapping
            //var players = RavenSession.Query<Player>().Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5))).ToList();

            //var vm = Mapper.Map<IEnumerable<PlayerViewModel>>(players);
            //return View(vm);

            var results = RavenSession.Query<Player_MatchResults.Result, Player_MatchResults>().OrderByDescending(x => x.Wins).ThenByDescending(x => x.Draws).ThenByDescending(x => x.Extras).ToList();

            return View(results);
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
    }
}