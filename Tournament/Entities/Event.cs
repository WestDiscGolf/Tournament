using System.Collections.Generic;

namespace Tournament.Entities
{
    public class Event : RootAggregate
    {
        public string Name { get; set; }
        public string Slug { get; set; }

        public ICollection<string> TeamIds { get; set; }
        public ICollection<string> LegIds { get; set; }

        // todo
        //public ICollection<Comment> Comments { get; set; } 
    }
}