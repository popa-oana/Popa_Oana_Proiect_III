using Proiect_Netficks.Models;

namespace Proiect_Netficks.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Film> PopularFilms { get; set; } = new List<Film>();
        public IEnumerable<Film> NewestFilms { get; set; } = new List<Film>();
        public IEnumerable<Serial> PopularSerials { get; set; } = new List<Serial>();
        public IEnumerable<Serial> NewestSerials { get; set; } = new List<Serial>();
        public IEnumerable<Lista_Mea> Watchlist { get; set; } = new List<Lista_Mea>();
    }
}
