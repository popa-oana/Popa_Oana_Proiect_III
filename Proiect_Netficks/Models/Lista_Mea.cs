using Proiect_Netficks.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_Netficks.Models
{
    public class Lista_Mea
    {
        [Key]
        public int Lista_ID { get; set; }

        public int Utilizator_ID { get; set; }

        public int? Film_ID { get; set; }

        public int? Serial_ID { get; set; }

        public DateTime Data_Creare { get; set; }

        // Navigation properties
        [ForeignKey("Utilizator_ID")]
        public virtual Utilizator Utilizator { get; set; }

        [ForeignKey("Film_ID")]
        public virtual Film Film { get; set; }

        [ForeignKey("Serial_ID")]
        public virtual Serial Serial { get; set; }
    }
}
