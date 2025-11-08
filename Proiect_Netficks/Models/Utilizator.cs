using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.Models
{
    public class Utilizator
    {
        [Key]
        public int Utilizator_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nume { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Parola { get; set; }

        public DateTime Data_Inregistrare { get; set; }

        public string Tip_Abonament { get; set; }

        // Navigation properties
        public virtual ICollection<Abonament> Abonamente { get; set; }
        public virtual ICollection<Istoric_Vizionari> IstoricVizionari { get; set; }
        public virtual ICollection<Lista_Mea> ListaMea { get; set; }
        public virtual ICollection<Recenzii> Recenzii { get; set; }
    }
}