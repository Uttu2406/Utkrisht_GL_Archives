using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Data;
using TransportManagement.Exceptions;
using TransportManagement.Models;

namespace TransportManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehiclesController : Controller
    {
        private readonly TransportDbContext _context;

        public VehiclesController(TransportDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return View(vehicles);
        }

        // Logic updated: Throw exception directly so Unit Tests can catch it.
        public async Task<IActionResult> Details(int id)
        {
            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleID == id);

            if (vehicle == null)
            {
                throw new VehicleNotFoundException($"Vehicle ID {id} does not exist.");
            }

            return View(vehicle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddVehicle(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Vehicles.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await _context.Vehicles.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVehicle(Vehicle vehicle)
        {
            var exists = await _context.Vehicles.AnyAsync(v => v.VehicleID == vehicle.VehicleID);
            if (!exists)
            {
                throw new VehicleNotFoundException($"Cannot update: Vehicle with ID {vehicle.VehicleID} not found.");
            }

            _context.Update(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
            {
                throw new VehicleNotFoundException($"Cannot delete: Vehicle with ID {vehicleId} not found.");
            }

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}