using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.Models
{
    public enum VizionareStatus
    {
        [Display(Name = "Vreau să văd")]
        VreauSaVad = 1,
        
        [Display(Name = "Început")]
        Inceput = 2,
        
        [Display(Name = "În curs")]
        InCurs = 3,
        
        [Display(Name = "Vizionat")]
        Vizionat = 4
    }
}
