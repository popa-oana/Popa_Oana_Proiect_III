using Proiect_Netficks.Models;

namespace Proiect_Netficks.ViewModels
{
    public class VizionareFilmViewModel
    {
        public Film Film { get; set; } = null!;
    }

    public class VizionareEpisodViewModel
    {
        public Serial Serial { get; set; } = null!;
        public Episod Episod { get; set; } = null!;
    }
}
