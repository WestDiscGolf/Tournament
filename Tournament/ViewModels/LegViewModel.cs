using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Tournament.ViewModels
{
    public class LegViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        public LocationViewModel Location { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        public string HomeCaptainId { get; set; }
        public PlayerViewModel HomeCaptain { get; set; }
        [Required]
        public string AwayCaptainId { get; set; }
        public PlayerViewModel AwayCaptain { get; set; }
        [Required]
        public string HomeTeamId { get; set; }
        public TeamViewModel HomeTeam { get; set; }
        [Required]
        public string AwayTeamId { get; set; }
        public TeamViewModel AwayTeam { get; set; }

        [Required]
        public string EventId { get; set; }

        public string[] TeamIds { get; set; }

        //public ICollection<string> MatchIds { get; set; }
        //public ICollection<MatchViewModel> Matches { get; set; }
        public ICollection<ExtraViewModel> Extras { get; set; }

        public IEnumerable<SelectListItem> PlayersDataSource { get; set; }
        public IEnumerable<SelectListItem> TeamDataSource { get; set; }
    }
}