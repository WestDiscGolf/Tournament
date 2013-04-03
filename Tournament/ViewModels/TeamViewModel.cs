using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tournament.ViewModels
{
    public class TeamViewModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Website { get; set; }
        public string Logo { get; set; }
        [Required]
        public string Slug { get; set; }
        public LocationViewModel HomeCourse { get; set; }
    }
}