using System.ComponentModel.DataAnnotations;

namespace Tournament.Entities
{
    public class Team : RootAggregate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Website { get; set; }
        public string Logo { get; set; }
        [Required]
        public string Slug { get; set; }
        public Location Course { get; set; }
    }
}