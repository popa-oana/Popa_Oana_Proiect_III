using Proiect_Netficks.Models;
using System.Collections.Generic;

namespace Proiect_Netficks.ViewModels
{
    public class TopViewModel
    {
        public List<Film> TopFilms { get; set; } = new List<Film>();
        public List<Serial> TopSeries { get; set; } = new List<Serial>();
    }
}
