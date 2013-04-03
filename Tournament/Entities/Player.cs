namespace Tournament.Entities
{
    public class Player : RootAggregate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Twitter { get; set; }
        public string Slug { get; set; }
    }
}