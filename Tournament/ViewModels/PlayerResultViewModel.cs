using System.ComponentModel;

namespace Tournament.ViewModels
{
    public class PlayerResultViewModel
    {
        public PlayerViewModel Player { get; set; }

        [DisplayName("wins")]
        public int Wins { get; set; }

        [DisplayName("Draws")]
        public int Draws { get; set; }

        [DisplayName("Losses")]
        public int Losses { get; set; }

        [DisplayName("Extras")]
        public int Extras { get; set; }
    }
}