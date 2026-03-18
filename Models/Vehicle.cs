using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportManagement.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }

        [Required]
        [StringLength(255)]
        public string Model { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Capacity { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
