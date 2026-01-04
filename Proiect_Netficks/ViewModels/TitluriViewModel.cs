using Proiect_Netficks.Models;
using System.Collections.Generic;

namespace Proiect_Netficks.ViewModels
{
    public class TitluriViewModel
    {
        public IEnumerable<Film> Filme { get; set; } = new List<Film>();
        public IEnumerable<Serial> Seriale { get; set; } = new List<Serial>();
    }
}
