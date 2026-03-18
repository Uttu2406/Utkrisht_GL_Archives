using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransportManagement.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        // Foreign Key to Trip
        [Required]
        public int TripID { get; set; }

        [ForeignKey("TripID")]
        public Trip Trip { get; set; }

        // Foreign Key to Passenger
        [Required]
        public int PassengerID { get; set; }

        [ForeignKey("PassengerID")]
        public Passenger Passenger { get; set; }

        public DateTime BookingDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }
    }
}
