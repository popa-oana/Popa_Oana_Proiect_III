// Models/Serial.cs
using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_Netficks.Models
{
    public class Serial
    {
        [Key]
        public int Serial_ID { get; set; }

        public int Gen_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Titlu { get; set; }

        public int An_Aparitie { get; set; }

        public int Numar_Sezoane { get; set; }

        public string Descriere { get; set; }

        // Navigation properties
        [ForeignKey("Gen_ID")]
        public virtual Gen Gen { get; set; }
        public virtual ICollection<Episod> Episoade { get; set; }
        public virtual ICollection<Lista_Mea> ListaMea { get; set; }
    }
}
