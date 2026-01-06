using System;
using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.Models
{
    public class ListaVizionare
    {
        [Key]
        public int ListaVizionare_ID { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        public int? Film_ID { get; set; }
        public Film? Film { get; set; }
        
        public int? Serial_ID { get; set; }
        public Serial? Serial { get; set; }
        
        public DateTime DataAdaugare { get; set; } = DateTime.Now;
    }
}
