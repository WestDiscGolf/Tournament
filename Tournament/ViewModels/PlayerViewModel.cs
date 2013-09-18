using System.ComponentModel.DataAnnotations;

namespace Tournament.ViewModels
{
    public class PlayerViewModel
    {
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Twitter { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string TeamId { get; set; }
        public TeamViewModel Team { get; set; }

        public string Description { get; set; }

        public string FullName
        {
            get { return FirstName + " " + (string.IsNullOrWhiteSpace(NickName) ? "" : "\"" + NickName + "\" ") + LastName; }
        }
    }
}