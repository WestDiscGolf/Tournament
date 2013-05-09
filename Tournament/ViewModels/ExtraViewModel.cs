using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Tournament.ViewModels
{
    public class ExtraViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Player must be selected")]
        public string PlayerId { get; set; }
        public PlayerViewModel Player { get; set; }

        [Required(ErrorMessage = "Team must be selected")]
        public string TeamId { get; set; }
        public TeamViewModel Team { get; set; }

        public string LegId { get; set; }
        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }
        public IEnumerable<SelectListItem> PlayersDataSource { get; set; }
        public IEnumerable<SelectListItem> TeamDataSource { get; set; }
    }
}