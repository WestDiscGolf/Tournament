//using System.Linq;
//using Raven.Client.Indexes;
//using Tournament.Entities;

//namespace Tournament.Infrastructure.Indexes
//{
//    public class Player_TeamCaptain : AbstractMultiMapIndexCreationTask<Player_TeamCaptain.Result>
//    {
//        public class Result
//        {
//            public string TeamId { get; set; }
//            public string CaptainId { get; set; }
//            public Player Captain { get; set; }
//        }

//        public Player_TeamCaptain()
//        {
//            AddMap<Leg>(legs => from item in legs
//                                select new Result
//                                    {
//                                        CaptainId = item.HomeCaptain.Id,
//                                        TeamId = item.HomeTeam.Id
//                                    });

//            AddMap<Leg>(legs => from item in legs
//                                select new Result
//                                {
//                                    CaptainId = item.AwayCaptain.Id,
//                                    TeamId = item.AwayTeam.Id
//                                });

//            TransformResults = (database, results) => from result in results
//                                                      let player = database.Load<Player>(result.CaptainId)
//                                                      select new Result
//                                                          {
//                                                              TeamId = result.TeamId,
//                                                              Captain = player
//                                                          };
//        }
//    }
//}