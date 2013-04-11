using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.Enumerations;
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

        //[Authorize]
        [HttpGet]
        public ActionResult Create(string homeTeamId = "", string awayTeamId = "")
        {
            var vm = new MatchViewModel
                {
                    HomeTeamId = homeTeamId,
                    AwayTeamId = awayTeamId
                };

            InitialiseReferenceData(vm);

            return View(vm);
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MatchViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<Match>(viewModel);
                // determine win
                if (string.IsNullOrWhiteSpace(viewModel.WinningTeamId))
                {
                    entity.Result = Result.Draw;
                }
                else
                {
                    if (viewModel.HomeTeamId == viewModel.WinningTeamId)
                    {
                        entity.Result = Result.HomeWin;
                    }
                    if (viewModel.AwayTeamId == viewModel.WinningTeamId)
                    {
                        entity.Result = Result.AwayWin;
                    }
                    entity.WinningTeam = RavenSession.Load<Team>(viewModel.WinningTeamId);
                }

                entity.HomeTeam = RavenSession.Load<Team>(viewModel.HomeTeamId);
                entity.AwayTeam = RavenSession.Load<Team>(viewModel.AwayTeamId);
                entity.HomePlayers = RavenSession.Load<Player>(viewModel.HomePlayerIds);
                entity.AwayPlayers = RavenSession.Load<Player>(viewModel.AwayPlayerIds);
                RavenSession.Store(entity, "matches/");
                return RedirectToAction("Index");
            }

            InitialiseReferenceData(viewModel);
            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var entity = RavenSession.Load<Match>(id);
            if (entity == null)
            {
                return HttpNotFound(string.Format("Match {0} does not exist", id));
            }

            var vm = Mapper.Map<MatchViewModel>(entity);
            vm.HomePlayerIds = vm.HomePlayers.Select(x => x.Id).ToList();
            vm.AwayPlayerIds = vm.AwayPlayers.Select(x => x.Id).ToList();

            InitialiseReferenceData(vm);
            return View(vm);
        }

        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MatchViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<Match>(viewModel);
                // determine win
                if (string.IsNullOrWhiteSpace(viewModel.WinningTeamId))
                {
                    entity.Result = Result.Draw;
                }
                else
                {
                    if (viewModel.HomeTeamId == viewModel.WinningTeamId)
                    {
                        entity.Result = Result.HomeWin;
                    }
                    if (viewModel.AwayTeamId == viewModel.WinningTeamId)
                    {
                        entity.Result = Result.AwayWin;
                    }
                    entity.WinningTeam = RavenSession.Load<Team>(viewModel.WinningTeamId);
                }

                entity.HomeTeam = RavenSession.Load<Team>(viewModel.HomeTeamId);
                entity.AwayTeam = RavenSession.Load<Team>(viewModel.AwayTeamId);
                entity.HomePlayers = RavenSession.Load<Player>(viewModel.HomePlayerIds);
                entity.AwayPlayers = RavenSession.Load<Player>(viewModel.AwayPlayerIds);
                return RedirectToAction("Index");
            }
            InitialiseReferenceData(viewModel);
            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var model = RavenSession.Load<Match>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Match {0} does not exist", id));
            }
            return View(Mapper.Map<MatchViewModel>(model));
        }

        //[Authorize]
        [HttpPost]
        [ActionName("delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(string id)
        {
            var model = RavenSession.Load<Match>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Match {0} does not exist", id));
            }
            RavenSession.Delete(model);
            return RedirectToAction("Index");
        }

        private void InitialiseReferenceData(MatchViewModel viewModel)
        {
            viewModel.HomeTeamsDataSource  = new[] { new SelectListItem { Text = "", Value = "" } }.Union(from team in RavenSession.Query<Team>().OrderBy(x => x.Name).ToList()
                                                                                                          select new SelectListItem
                                                                                                          {
                                                                                                              Text = team.Name,
                                                                                                              Value = team.Id,
                                                                                                              Selected = (string.Compare(team.Id, viewModel.HomeTeamId, StringComparison.InvariantCultureIgnoreCase) == 0)
                                                                                                          });

            viewModel.AwayTeamsDataSource = new[] { new SelectListItem { Text = "", Value = "" } }.Union(from team in RavenSession.Query<Team>().OrderBy(x => x.Name).ToList()
                                                                                                         select new SelectListItem
                                                                                                         {
                                                                                                             Text = team.Name,
                                                                                                             Value = team.Id,
                                                                                                             Selected = (string.Compare(team.Id, viewModel.HomeTeamId, StringComparison.InvariantCultureIgnoreCase) == 0)
                                                                                                         });

            viewModel.WinningTeamsDataSource = new[] { new SelectListItem { Text = "", Value = "" } }.Union(from team in RavenSession.Query<Team>().OrderBy(x => x.Name).ToList()
                                                                                                            select new SelectListItem
                                                                                                            {
                                                                                                                Text = team.Name,
                                                                                                                Value = team.Id,
                                                                                                                Selected = (string.Compare(team.Id, viewModel.WinningTeamId, StringComparison.InvariantCultureIgnoreCase) == 0)
                                                                                                            });

            var players = RavenSession.Query<Player>().ToList();
            var vm = Mapper.Map<IEnumerable<PlayerViewModel>>(players).ToList();
            viewModel.HomePlayersDataSource = new[] {new SelectListItem {Text = "", Value = ""}}
                .Union(vm.Select(p => new SelectListItem { Text = p.FullName, Value = p.Id } ));

            viewModel.AwayPlayersDataSource = new[] { new SelectListItem { Text = "", Value = "" } }
                .Union(vm.Select(p => new SelectListItem { Text = p.FullName, Value = p.Id }));

        }
    }
}