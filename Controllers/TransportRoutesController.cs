using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportManagement.Data;
using TransportManagement.Models;

namespace TransportManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransportRoutesController : Controller
    {
        private readonly TransportDbContext _context;

        public TransportRoutesController(TransportDbContext context) => _context = context;

        // GET: TransportRoutes (Fixes 404 when clicking "Routes")
        public async Task<IActionResult> Index()
        {
            return View(await _context.Routes.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoute([Bind("RouteID,StartDestination,EndDestination,Distance")] TransportRoute transportRoute)
        {
            if (ModelState.IsValid)
            {
                _context.Routes.Add(transportRoute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await _context.Routes.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoute(int id, TransportRoute transportRoute)
        {
            if (id != transportRoute.RouteID) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(transportRoute);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index", await _context.Routes.ToListAsync());
        }

        [HttpPost, ActionName("DeleteRoute")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route != null)
            {
                _context.Routes.Remove(route);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}