using System.Collections.Generic;
using Tournament.Enumerations;

namespace Tournament.Entities
{
    public class Match : RootAggregate
    {
        // todo: link to leg
        public Classification Classification { get; set; }
        public Result Result { get; set; }
        public Team WinningTeam { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public ICollection<Player> HomePlayers { get; set; }
        public ICollection<Player> AwayPlayers { get; set; }
        
        // todo: come back to comments
        public ICollection<Comment> Comments { get; set; }
    }
}