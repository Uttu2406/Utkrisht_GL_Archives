using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportManagement.Models
{
    public class Trip
    {
        [Key]
        public int TripID { get; set; }

        // Foreign Key to Vehicle
        [Required]
        public int VehicleID { get; set; }

        [ForeignKey("VehicleID")]
        public Vehicle Vehicle { get; set; }

        // Foreign Key to Route
        [Required]
        public int RouteID { get; set; }

        [ForeignKey("RouteID")]
        public TransportRoute Route { get; set; }

        [Required]
        public DateTime DepartureDate { get; set; }

        [Required]
        public DateTime ArrivalDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string TripType { get; set; } = "Freight";

        public int? MaxPassengers { get; set; }
    }
}
