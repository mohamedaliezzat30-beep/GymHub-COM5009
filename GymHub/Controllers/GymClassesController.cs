using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymHub.Data;
using GymHub.Models;

namespace GymHub.Controllers
{
    [Authorize]
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymClassesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bookingCounts = await _context.Bookings
                .Where(b => b.BookingStatus == "Confirmed")
                .GroupBy(b => b.GymClassId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            ViewBag.BookingCounts = bookingCounts;

            return View(await _context.GymClasses.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.GymClassId == id);

            if (gymClass == null) return NotFound();

            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GymClassId,ClassName,ClassDate,StartTime,EndTime,Instructor,DifficultyLevel,Capacity")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.GymClasses.Add(gymClass);

                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = User.Identity?.Name,
                    Action = "Admin added scheduled class: " + gymClass.ClassName,
                    DateCreated = DateTime.Now
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var gymClass = await _context.GymClasses.FindAsync(id);

            if (gymClass == null) return NotFound();

            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GymClassId,ClassName,ClassDate,StartTime,EndTime,Instructor,DifficultyLevel,Capacity")] GymClass gymClass)
        {
            if (id != gymClass.GymClassId) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(gymClass);

                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = User.Identity?.Name,
                    Action = "Admin updated scheduled class: " + gymClass.ClassName,
                    DateCreated = DateTime.Now
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var gymClass = await _context.GymClasses
                .FirstOrDefaultAsync(m => m.GymClassId == id);

            if (gymClass == null) return NotFound();

            return View(gymClass);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);

            if (gymClass != null)
            {
                _context.ActivityLogs.Add(new ActivityLog
                {
                    UserEmail = User.Identity?.Name,
                    Action = "Admin deleted scheduled class: " + gymClass.ClassName,
                    DateCreated = DateTime.Now
                });

                _context.GymClasses.Remove(gymClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}