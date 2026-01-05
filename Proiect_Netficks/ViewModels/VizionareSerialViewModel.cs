using Proiect_Netficks.Models;
using System.Collections.Generic;

namespace Proiect_Netficks.ViewModels
{
    public class VizionareSerialViewModel
    {
        public Serial Serial { get; set; } = null!;
        
        // Group episodes by season for display
        public Dictionary<int, List<Episod>> EpisoadeGrupate { get; set; } = new Dictionary<int, List<Episod>>();
    }
}
