using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tournament.ViewModels
{
    public class EventViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Slug { get; set; }

        [Required]
        public ICollection<string> TeamIds { get; set; }
        public IEnumerable<TeamViewModel> Teams { get; set; }

        public IDictionary<string, double> TeamScores { get; set; } 
    }
}