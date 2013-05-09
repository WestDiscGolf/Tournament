namespace Tournament.Entities
{
    public class Extra
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Player Player { get; set; }
        public string PlayerId { get; set; }
        public Team Team { get; set; }
        public string TeamId { get; set; }
    }
}