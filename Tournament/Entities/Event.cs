using System.Collections.Generic;

namespace Tournament.Entities
{
    public class Event : RootAggregate
    {
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<Team> Teams { get; set; } 

        // legs?
        // comments?
    }
}