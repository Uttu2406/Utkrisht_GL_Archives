using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Data;
using TransportManagement.Models;

namespace TransportManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PassengersController : Controller
    {
        private readonly TransportDbContext _context;

        public PassengersController(TransportDbContext context)
        {
            _context = context;
        }

        // FIX: Added Index method so the page loads when you click "Passengers" in the menu
        public async Task<IActionResult> Index()
        {
            var passengers = await _context.Passengers.ToListAsync();
            return View(passengers); // Looks for Views/Passengers/Index.cshtml
        }

        // ▪ Management: Adding a passenger
        [HttpPost]
        public async Task<IActionResult> AddPassenger(Passenger passenger)
        {
            if (ModelState.IsValid)
            {
                _context.Passengers.Add(passenger);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redirect back to the list
            }
            return View("Index", await _context.Passengers.ToListAsync());
        }

        // ▪ Management: Updating a passenger
        [HttpPost]
        public async Task<IActionResult> UpdatePassenger(Passenger passenger)
        {
            _context.Update(passenger);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ▪ Booking Retrieval: getBookingsByPassenger
        // Requirement: Throw exception if no bookings exist for the passenger
        [HttpGet]
        public async Task<IActionResult> GetBookingsByPassenger(int passengerId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.PassengerID == passengerId)
                .ToListAsync();

            if (bookings == null || !bookings.Any())
            {
                // Note: Replace with your custom BookingNotFoundException once the folder is ready
                throw new Exception("BookingNotFoundException: No bookings found for this passenger.");
            }

            return View("Index", bookings); // Shows the specific bookings on the passenger page
        }
    }
}