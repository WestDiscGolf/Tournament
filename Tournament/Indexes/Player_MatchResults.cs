using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Tournament.Entities;

namespace Tournament.Indexes
{
    public class Player_MatchResults : AbstractMultiMapIndexCreationTask<Player_MatchResults.Result>
    {
        public class Result
        {
            public string PlayerId { get; set; }
            public Player Player { get; set; }
            public int Wins { get; set; }
            public int Losses { get; set; }
            public int Draws { get; set; }
            public int Extras { get; set; }
        }

        public Player_MatchResults()
        {
            AddMap<Match>(matches => from match in matches
                                     from player in match.HomePlayers.Select(x => x)
                                     where match.Result == Enumerations.Result.HomeWin
                                     select new Result
                                         {
                                             PlayerId = player.Id,
                                             Wins = 1,
                                             Losses = 0,
                                             Draws = 0,
                                             Extras = 0
                                         }
                );

            AddMap<Match>(matches => from match in matches
                                     from player in match.AwayPlayers.Select(x => x)
                                     where match.Result == Enumerations.Result.HomeWin
                                     select new Result
                                     {
                                         PlayerId = player.Id,
                                         Wins = 0,
                                         Losses = 1,
                                         Draws = 0,
                                         Extras = 0
                                     }
                );

            AddMap<Match>(matches => from match in matches
                                     from player in match.HomePlayers.Select(x => x)
                                     where match.Result == Enumerations.Result.AwayWin
                                     select new Result
                                     {
                                         PlayerId = player.Id,
                                         Wins = 0,
                                         Losses = 1,
                                         Draws = 0,
                                         Extras = 0
                                     }
                );

            AddMap<Match>(matches => from match in matches
                                     from player in match.AwayPlayers.Select(x => x)
                                     where match.Result == Enumerations.Result.AwayWin
                                     select new Result
                                     {
                                         PlayerId = player.Id,
                                         Wins = 1,
                                         Losses = 0,
                                         Draws = 0,
                                         Extras = 0
                                     }
                );

            AddMap<Match>(matches => from match in matches
                                     from player in match.HomePlayers.Select(x => x).Union(match.AwayPlayers.Select(x => x))
                                     where match.Result == Enumerations.Result.Draw
                                     select new Result
                                     {
                                         PlayerId = player.Id,
                                         Wins = 0,
                                         Losses = 0,
                                         Draws = 1,
                                         Extras = 0
                                     }
                );

            AddMap<Leg>(legs => from leg in legs
                                from extra in leg.Extras
                                select new Result
                                    {
                                        PlayerId = extra.PlayerId,
                                        Wins = 0,
                                        Losses = 0,
                                        Draws = 0,
                                        Extras = 1
                                    }
                );

            Reduce = results => from result in results
                                group result by result.PlayerId
                                into r
                                select new Result
                                    {
                                        PlayerId = r.Key,
                                        Wins = r.Sum(x => x.Wins),
                                        Losses = r.Sum(x => x.Losses),
                                        Draws = r.Sum(x => x.Draws),
                                        Extras = r.Sum(x => x.Extras)
                                    };

            TransformResults = (database, results) => from result in results
                                                      let player = database.Load<Player>(result.PlayerId)
                                                      select new Result
                                                      {
                                                          PlayerId = result.PlayerId,
                                                          Player = player,
                                                          Wins = result.Wins,
                                                          Losses = result.Losses,
                                                          Draws = result.Draws,
                                                          Extras = result.Extras
                                                      };


            Index(x => x.PlayerId, FieldIndexing.Analyzed);
            Index(x => x.Wins, FieldIndexing.Default);
            Index(x => x.Losses, FieldIndexing.Default);
            Index(x => x.Draws, FieldIndexing.Default);
            Index(x => x.Extras, FieldIndexing.Default);
        }
    }
}