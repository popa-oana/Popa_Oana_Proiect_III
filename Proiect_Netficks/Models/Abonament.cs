using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proiect_Netficks.Models
{
    public class Abonament
    {
        [Key]
        public int Abonament_ID { get; set; }

        public int Utilizator_ID { get; set; }

        [Required]
        public string Tip { get; set; }

        public DateTime Data_Start { get; set; }

        public DateTime Data_Sfarsit { get; set; }

        public string Status { get; set; }

        // Navigation property
        [ForeignKey("Utilizator_ID")]
        public virtual Utilizator Utilizator { get; set; }
    }
}