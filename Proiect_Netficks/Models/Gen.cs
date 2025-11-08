using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Proiect_Netficks.Models
{
    public class Gen
    {
        [Key]
        public int Gen_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nume_Gen { get; set; }

        // Navigation properties
        public virtual ICollection<Film> Filme { get; set; }
        public virtual ICollection<Serial> Seriale { get; set; }
    }
}