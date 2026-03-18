using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Data;
using TransportManagement.Models;

namespace TransportManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TripsController : Controller
    {
        private readonly TransportDbContext _context;

        public TripsController(TransportDbContext context) => _context = context;

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            // FIX: Added .Include to ensure Vehicle and Route data is loaded
            var trips = await _context.Trips
                .Include(t => t.Vehicle)
                .Include(t => t.Route)
                .OrderByDescending(t => t.DepartureDate)
                .ToListAsync();

            return View(trips);
        }

        // POST: ScheduleTrip
        [HttpPost]
        public async Task<IActionResult> ScheduleTrip(int vehicleId, int routeId, DateTime departureDate, DateTime arrivalDate)
        {
            var trip = new Trip
            {
                VehicleID = vehicleId,
                RouteID = routeId,
                DepartureDate = departureDate,
                ArrivalDate = arrivalDate,
                Status = "Scheduled"
            };

            _context.Trips.Add(trip);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: CancelTrip
        [HttpPost]
        public async Task<IActionResult> CancelTrip(int tripId)
        {
            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null) return NotFound();

            trip.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}