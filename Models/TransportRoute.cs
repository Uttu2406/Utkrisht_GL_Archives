using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportManagement.Models
{
    public class TransportRoute
    {
        [Key]
        public int RouteID { get; set; }

        [Required]
        [StringLength(255)]
        public string StartDestination { get; set; }

        [Required]
        [StringLength(255)]
        public string EndDestination { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Distance { get; set; }
    }
}
