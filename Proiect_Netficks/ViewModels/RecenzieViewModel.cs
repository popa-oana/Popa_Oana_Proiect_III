using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.ViewModels
{
    public class RecenzieViewModel
    {
        public int? Film_ID { get; set; }
        public int? Serial_ID { get; set; }
        public int? Episod_ID { get; set; }
        
        [Required(ErrorMessage = "Nota este obligatorie")]
        [Range(1, 10, ErrorMessage = "Nota trebuie să fie între 1 și 10")]
        [Display(Name = "Notă (1-10)")]
        public int Nota { get; set; }
        
        [Required(ErrorMessage = "Comentariul este obligatoriu")]
        [StringLength(1000, MinimumLength = 5, ErrorMessage = "Comentariul trebuie să aibă între 5 și 1000 de caractere")]
        [Display(Name = "Comentariu")]
        public string Comentariu { get; set; } = string.Empty;
    }
}
