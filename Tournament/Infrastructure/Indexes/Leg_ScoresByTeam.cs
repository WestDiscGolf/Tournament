using System.Linq;
using Raven.Client.Indexes;
using Tournament.Entities;

namespace Tournament.Infrastructure.Indexes
{
    public class Leg_ScoresByTeam : AbstractMultiMapIndexCreationTask<Leg_ScoresByTeam.Result>
    {
        public class Result
        {
            public string LegId { get; set; }
            public Leg Leg { get; set; }
            public string TeamId { get; set; }
            public Team Team { get; set; }
            public int Wins { get; set; }
            public int Draws { get; set; }
            public int Extras { get; set; }
            public double Total { get; set; }
        }

        public Leg_ScoresByTeam()
        {
            // wins
            AddMap<Match>(matches => from match in matches
                                     where match.WinningTeam != null
                                     select new Result
                                         {
                                             LegId = match.LegId,
                                             TeamId = match.WinningTeam.Id,
                                             Wins = 1,
                                             Draws = 0,
                                             Extras = 0,
                                             Total = 0
                                         }
                );

            // draws (home team)
            AddMap<Match>(matches => from match in matches
                                     where match.WinningTeam == null
                                     select new Result
                                         {
                                             LegId = match.LegId,
                                             TeamId = match.HomeTeam.Id,
                                             Wins = 0,
                                             Draws = 1,
                                             Extras = 0,
                                             Total = 0
                                         }
                );

            // draws (away team)
            AddMap<Match>(matches => from match in matches
                                     where match.WinningTeam == null
                                     select new Result
                                         {
                                             LegId = match.LegId,
                                             TeamId = match.AwayTeam.Id,
                                             Wins = 0,
                                             Draws = 1,
                                             Extras = 0,
                                             Total = 0
                                         }
                );

            // extras
            AddMap<Leg>(legs => from leg in legs
                                from extra in leg.Extras
                                select new Result
                                    {
                                        LegId = leg.Id,
                                        TeamId = extra.TeamId,
                                        Wins = 0,
                                        Draws = 0,
                                        Extras = 1,
                                        Total = 0
                                    }
                );

            // reduce
            Reduce = results => from result in results
                                group result by new { result.TeamId, result.LegId }
                                into r
                                select new Result
                                    {
                                        TeamId = r.Key.TeamId,
                                        LegId = r.Key.LegId,
                                        Wins = r.Sum(x => x.Wins),
                                        Draws = r.Sum(x => x.Draws),
                                        Extras = r.Sum(x => x.Extras),
                                        Total = r.Sum(x => x.Wins) + r.Sum(x => x.Draws * 0.5) + r.Sum(x => x.Extras * 0.5)
                                    };

            // transform
            TransformResults = (database, results) => from result in results
                                                      let team = database.Load<Team>(result.TeamId)
                                                      let leg = database.Load<Leg>(result.LegId)
                                                      select new Result
                                                          {
                                                              TeamId = result.TeamId,
                                                              Team = team,
                                                              LegId = result.LegId,
                                                              Leg = leg,
                                                              Wins = result.Wins,
                                                              Draws = result.Draws,
                                                              Extras = result.Extras,
                                                              Total = result.Total
                                                          };

        }
    }
}