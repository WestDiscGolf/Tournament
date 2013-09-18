using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Raven.Client.Indexes;
using Tournament.Entities;
using Tournament.Enumerations;

namespace Tournament.Infrastructure.Indexes
{
    public class Match_ByPlayer : AbstractMultiMapIndexCreationTask<Match_ByPlayer.Result>
    {
        public class Result
        {
            public string PlayerId { get; set; }
            public string MatchId { get; set; }
            public Classification Classification { get; set; }
        }

        public Match_ByPlayer()
        {
            AddMap<Match>(matches => from match in matches
                                     from player in match.HomePlayers
                                     select new Result
                                         {
                                             PlayerId = player.Id,
                                             MatchId = match.Id,
                                             Classification = match.Classification
                                         });

            AddMap<Match>(matches => from match in matches
                                     from player in match.AwayPlayers
                                     select new Result
                                     {
                                         PlayerId = player.Id,
                                         MatchId = match.Id,
                                         Classification = match.Classification
                                     });

            Reduce = results => from result in results
                                group result by new {result.PlayerId, result.MatchId, result.Classification}
                                into r
                                select new Result()
                                    {
                                        PlayerId = r.Key.PlayerId,
                                        MatchId = r.Key.MatchId,
                                        Classification = r.Key.Classification
                                    };
        }
    }
}