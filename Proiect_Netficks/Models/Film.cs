using Proiect_Netficks.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_Netficks.Models
{
    public class Film
    {
        [Key]
        public int Film_ID { get; set; }

        public int Gen_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Titlu { get; set; }

        public int An_Lansare { get; set; }

        public int Durata { get; set; }

        public string Descriere { get; set; }
        [ForeignKey("Gen_ID")]
        public virtual Gen ? Gen  { get; set; }
        public virtual ICollection<Lista_Mea>? ListaMea { get; set; }
        public virtual ICollection<Istoric_Vizionari>? IstoricVizionari { get; set; }
        public virtual ICollection<Recenzii>? Recenzii { get; set; }
    }
}
