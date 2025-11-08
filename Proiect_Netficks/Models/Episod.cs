using Proiect_Netficks.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_Netficks.Models
{
    public class Episod
    {
        [Key]
        public int Episod_ID { get; set; }

        public int Serial_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Titlu { get; set; }

        public int Numar_Sezon { get; set; }

        public int Numar_Episod { get; set; }

        public int Durata { get; set; }

        // Navigation properties
        [ForeignKey("Serial_ID")]
        public virtual Serial Serial { get; set; }
        public virtual ICollection<Istoric_Vizionari> IstoricVizionari { get; set; }
        public virtual ICollection<Recenzii> Recenzii { get; set; }
    }
}