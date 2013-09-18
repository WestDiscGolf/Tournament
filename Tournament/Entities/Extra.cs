namespace Tournament.Entities
{
    public class Extra
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Player Player { get; set; }
        public string PlayerId { get; set; }
        public Team Team { get; set; }
        public string TeamId { get; set; }
        public string LegId { get; set; }
    }
}