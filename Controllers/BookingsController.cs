using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Data;
using TransportManagement.Models;

namespace TransportManagement.Controllers
{
    
    public class BookingsController : Controller
    {
        private readonly TransportDbContext _context;

        public BookingsController(TransportDbContext context) => _context = context;

        // FIX: Added Index method so the page actually loads when you click the menu!
        public async Task<IActionResult> Index()
        {
            // This retrieves all bookings to show on the page
            var bookings = await _context.Bookings.ToListAsync();
            return View(bookings);
        }

        // ▪ Booking trips for passengers
        [HttpPost]
        public async Task<IActionResult> BookTrip(int tripId, int passengerId)
        {
            var booking = new Booking
            {
                TripID = tripId,
                PassengerID = passengerId,
                BookingDate = DateTime.Now, //
                Status = "Confirmed" //
            };
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); // Return to page instead of showing 'true'
        }

        // ▪ Canceling trips for passengers
        [HttpPost]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            // Requirement: Throw custom exception
            if (booking == null) throw new Exception("BookingNotFoundException");

            booking.Status = "Cancelled"; //
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ▪ Retrieving bookings made by passengers
        [HttpGet]
        public async Task<IActionResult> GetBookingsByPassenger(int passengerId)
        {
            var results = await _context.Bookings
                .Where(b => b.PassengerID == passengerId)
                .ToListAsync();
            return View("Index", results); // Pass results to the Index view
        }

        // ▪ Retrieving bookings associated with specific trips
        [HttpGet]
        public async Task<IActionResult> GetBookingsByTrip(int tripId)
        {
            var results = await _context.Bookings
                .Where(b => b.TripID == tripId)
                .ToListAsync();
            return View("Index", results);
        }
    }
}