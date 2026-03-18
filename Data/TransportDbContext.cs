using Microsoft.EntityFrameworkCore;
using TransportManagement.Models;

namespace TransportManagement.Data
{
    public class TransportDbContext:DbContext
    {
        public TransportDbContext(DbContextOptions<TransportDbContext> options)
        : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Models.TransportRoute> Routes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
