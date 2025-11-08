// Models/Recenzii.cs
using Proiect_Netficks.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_Netficks.Models
{
    public class Recenzii
    {
        [Key]
        public int Recenzie_ID { get; set; }

        public int Utilizator_ID { get; set; }

        public int? Film_ID { get; set; }

        public int? Episod_ID { get; set; }

        public int Nota { get; set; }

        [StringLength(1000)]
        public string Comentariu { get; set; }

        public DateTime Data_Postarii { get; set; }

        // Navigation properties
        [ForeignKey("Utilizator_ID")]
        public virtual Utilizator Utilizator { get; set; }

        [ForeignKey("Film_ID")]
        public virtual Film Film { get; set; }

        [ForeignKey("Episod_ID")]
        public virtual Episod Episod { get; set; }
    }
}
