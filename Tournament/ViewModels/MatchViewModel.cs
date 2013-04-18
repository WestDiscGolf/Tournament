using System.Collections.Generic;
using System.Web.Mvc;
using Tournament.Enumerations;

namespace Tournament.ViewModels
{
    public class MatchViewModel
    {
        public string Id { get; set; }
        public Classification Classification { get; set; }
        public Result Result { get; set; }
        public string WinningTeamId { get; set; }
        public TeamViewModel WinningTeam { get; set; }
        public string HomeTeamId { get; set; }
        public TeamViewModel HomeTeam { get; set; }
        public string AwayTeamId { get; set; }
        public TeamViewModel AwayTeam { get; set; }
        public ICollection<string> HomePlayerIds { get; set; } 
        public ICollection<PlayerViewModel> HomePlayers { get; set; }
        public ICollection<string> AwayPlayerIds { get; set; } 
        public ICollection<PlayerViewModel> AwayPlayers { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }

        public IEnumerable<SelectListItem> HomeTeamsDataSource { get; set; }
        public IEnumerable<SelectListItem> AwayTeamsDataSource { get; set; }
        public IEnumerable<SelectListItem> HomePlayersDataSource { get; set; }
        public IEnumerable<SelectListItem> AwayPlayersDataSource { get; set; }
        public IEnumerable<SelectListItem> WinningTeamsDataSource { get; set; }
    }
}