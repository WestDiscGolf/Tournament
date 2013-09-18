using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tournament.Entities;
using Tournament.Infrastructure.Indexes;

namespace Tournament.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var lastLeg = RavenSession.Query<Leg>().OrderByDescending(x => x.Date).FirstOrDefault() ?? new Leg();

            var legs = RavenSession.Query<Leg>().Where(x => x.EventId == lastLeg.EventId);

            var TeamScores = new Dictionary<string, double>();

            // think this could be done in an index!
            if (legs.Any())
            {
                foreach (var leg in legs)
                {
                    var scores = RavenSession.Query<Leg_ScoresByTeam.Result, Leg_ScoresByTeam>().Where(x => x.LegId == leg.Id).ToList();
                    foreach (var result in scores)
                    {
                        if (TeamScores.ContainsKey(result.Team.Id))
                        {
                            TeamScores[result.TeamId] += result.Total;
                        }
                        else
                        {
                            TeamScores.Add(result.TeamId, result.Total);
                        }
                    }
                }    
            }
            
            //var id = TeamScores.OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault();
            //if (id == null)
            //{
                
            //}
            //var bestTeam = RavenSession.Load<Team>(id);

            ViewBag.BestTeam = null; // bestTeam;

            ViewBag.Stats = RavenSession.Query<Statistics_General.Result, Statistics_General>().FirstOrDefault() ?? new Statistics_General.Result();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}