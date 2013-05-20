using System.Linq;
using Raven.Client.Indexes;
using Tournament.Entities;

namespace Tournament.Infrastructure.Indexes
{
    public class Statistics_General : AbstractMultiMapIndexCreationTask<Statistics_General.Result>
    {
        public class Result
        {
            public int Events { get; set; }
            public int Legs { get; set; }
            public int Players { get; set; }
            public int Matches { get; set; }
            public int Extras { get; set; }
        }

        public Statistics_General()
        {
            // count the events
            AddMap<Event>(events => from e in events
                                    select new Result
                                        {
                                            Events = 1,
                                            Extras = 0,
                                            Legs = 0,
                                            Matches = 0,
                                            Players = 0
                                        });

            AddMap<Leg>(legs => from leg in legs
                                    select new Result
                                        {
                                            Events = 0,
                                            Extras = 0,
                                            Legs = 1,
                                            Matches = 0,
                                            Players = 0
                                        });

            AddMap<Extra>(extras => from extra in extras
                                    select new Result
                                        {
                                            Events = 0,
                                            Extras = 0,
                                            Legs = 1,
                                            Matches = 0,
                                            Players = 0
                                        });

            AddMap<Match>(matches => from match in matches
                                    select new Result
                                    {
                                        Events = 0,
                                        Extras = 0,
                                        Legs = 0,
                                        Matches = 1,
                                        Players = 0
                                    });

            AddMap<Player>(players => from player in players
                                    select new Result
                                    {
                                        Events = 0,
                                        Extras = 0,
                                        Legs = 0,
                                        Matches = 0,
                                        Players = 1
                                    });

            Reduce = statses => from result in statses
                                group result by "contant"
                                into g
                                select new Result
                                    {
                                        Events = g.Sum(x => x.Events),
                                        Extras = g.Sum(x => x.Extras),
                                        Legs = g.Sum(x => x.Legs),
                                        Matches = g.Sum(x => x.Matches),
                                        Players = g.Sum(x => x.Players)
                                    };
        }
    }
}