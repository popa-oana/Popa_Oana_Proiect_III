using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Nume { get; set; } = string.Empty;

        public DateTime Data_Inregistrare { get; set; } = DateTime.Now;

        public string Tip_Abonament { get; set; } = string.Empty;
        
        // Profile image path (null if using default)
        public string? ProfileImagePath { get; set; } = null;

        // Navigation properties
        public virtual ICollection<Abonament> Abonamente { get; set; } = new List<Abonament>();
        public virtual ICollection<Istoric_Vizionari> IstoricVizionari { get; set; } = new List<Istoric_Vizionari>();
        public virtual ICollection<Lista_Mea> ListaMea { get; set; } = new List<Lista_Mea>();
        public virtual ICollection<Recenzii> Recenzii { get; set; } = new List<Recenzii>();
    }
}

