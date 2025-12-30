using Proiect_Netficks.Models;
using System.Collections.Generic;

namespace Proiect_Netficks.ViewModels
{
    public class IstoricViewModel
    {
        public List<Istoric_Vizionari> IstoricFilme { get; set; } = new List<Istoric_Vizionari>();
        public List<Istoric_Vizionari> IstoricEpisoade { get; set; } = new List<Istoric_Vizionari>();
    }
}
