namespace Tournament.Entities
{
    public class Team : RootAggregate
    {
        public string Name { get; set; }
        public string Website { get; set; }
        public string Logo { get; set; }
        public string Slug { get; set; }
        public Location HomeCourse { get; set; }
    }
}