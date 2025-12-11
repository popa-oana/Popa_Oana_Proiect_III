// Models/Serial.cs
using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_Netficks.Models
{
    public class Serial
    {
        public Serial()
        {
            // Initialize collections
            Episoade = new List<Episod>();
            ListaMea = new List<Lista_Mea>();
            IstoricVizionari = new List<Istoric_Vizionari>();
            Recenzii = new List<Recenzii>();
        }

        [Key]
        public int Serial_ID { get; set; }

        public int Gen_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Titlu { get; set; } = string.Empty;

        public int An_Aparitie { get; set; }

        public int Numar_Sezoane { get; set; }

        public string Descriere { get; set; } = string.Empty;
        
        [StringLength(255)]
        public string? ImagineUrl { get; set; } = "/images/series-placeholder-1.jpg";
        
        [StringLength(255)]
        public string? TrailerUrl { get; set; }

        // Navigation properties
        [ForeignKey("Gen_ID")]
        public virtual Gen? Gen { get; set; }
        public virtual ICollection<Episod> Episoade { get; set; }
        public virtual ICollection<Lista_Mea> ListaMea { get; set; }
        public virtual ICollection<Istoric_Vizionari> IstoricVizionari { get; set; }
        public virtual ICollection<Recenzii> Recenzii { get; set; }
    }
}
