using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tournament.Entities
{
    public class Leg : RootAggregate
    {
        public Leg()
        {
            Extras = new Collection<Extra>();
        }

        public string Name { get; set; }
        public string Slug { get; set; }
        public Location Location { get; set; }
        public DateTime Date { get; set; }
        public Player HomeCaptain { get; set; }
        public Player AwayCaptain { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }

        public string EventId { get; set; }

        public ICollection<Extra> Extras { get; set; }
        public ICollection<Comment> Comments { get; set; } 
    }
}