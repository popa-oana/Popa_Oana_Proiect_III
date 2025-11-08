// Models/Istoric_Vizionari.cs
using Proiect_Netficks.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_Netficks.Models
{
    public class Istoric_Vizionari
    {
        [Key]
        public int Vizionare_ID { get; set; }

        public int Utilizator_ID { get; set; }

        public int? Film_ID { get; set; }

        public int? Episod_ID { get; set; }

        public int Timp_Vizionare { get; set; }

        public DateTime Data_Vizionare { get; set; }

        // Navigation properties
        [ForeignKey("Utilizator_ID")]
        public virtual Utilizator Utilizator { get; set; }

        [ForeignKey("Film_ID")]
        public virtual Film Film { get; set; }

        [ForeignKey("Episod_ID")]
        public virtual Episod Episod { get; set; }
    }
}
